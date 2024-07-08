using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;

public class PlayScene : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button btnStart;
    public Button btnTryAgain;
    public Button btnNext;
    public Text txtMessage;
    public Text txtLevel;
    public GameObject boardObj;
    public GameObject itemPrefab; // Prefab with Image component
    public GameObject congratulationObj;
    public AudioClip sfxCongratulation;
    public AudioClip sfxSwap;

    private static string[] levelNames = { "EASY", "MIDDLE", "HARD" };
    private string folderPath = "items"; // Folder path relative to Resources folder
    private List<Sprite> sprites;
    private Vector2 boardDimension;
    private int _gap = 40;
    private int _rows;
    private int _columns = 3;
    private int[] _items = null;
    private GameObject[] _objs = null;
    private int[] _itemsOriginal = null;
    private float _itemWidth, _itemHeight;
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    // Start is called before the first frame update
    void Start()
    {
        LoadSprites();

        Image itemImage = boardObj.GetComponent<Image>();
        boardDimension = Utils.GetDisplayedDimensions(itemImage);
        boardDimension.x -= _gap * 2;
        boardDimension.y -= _gap * 2;

        OnBtnTryAgain();
    }

    // Update is called once per frame
    void Update()
    {
        Utils.HandleBackButton();
    }

    void LoadSprites()
    {
        sprites = new List<Sprite>();
        Object[] loadedSprites = Resources.LoadAll(folderPath, typeof(Sprite));

        foreach (Object obj in loadedSprites)
        {
            sprites.Add(obj as Sprite);
        }
    }

    public void ClearChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void PrepareItems()
    {
        int count = _rows * _columns;
        if (_items == null || _items.Length != count)
        {
            _items = new int[count];
            _objs = new GameObject[count];
        }

        for (int i = 0; i < count; i++)
        {
            int index = i;
            if (i >= sprites.Count)
                index = Random.Range(0, sprites.Count);
            _items[i] = index;
        }

        Utils.Shuffle(_items);

        _itemsOriginal = (int[])_items.Clone();
    }

    (float x, float y) ItemPos2LocalPos(int index)
    {
        float xPos = (index % _columns) * _itemWidth + _itemWidth / 2 - boardDimension.x / 2;
        float yPos = (index / _columns) * _itemHeight + _itemHeight / 2 - boardDimension.y / 2;
        return (xPos, yPos);
    }

    int LocalPos2ItemPos(Vector2 pos)
    {
        pos.x += boardDimension.x / 2;
        pos.y += boardDimension.y / 2;

        if (pos.x < 0 || pos.x >= boardDimension.x || pos.y < 0 || pos.y >= boardDimension.y)
            return -1;

        return (int)(pos.x / _itemWidth) + (int)(pos.y / _itemHeight) * _columns;
    }

    void ArrangeSpritesInGrid()
    {
        DOTween.CompleteAll();
        DOTween.KillAll();

        ClearChildren(boardObj.transform);

        // Calculate the size for each sprite item
        _itemWidth = boardDimension.x / _columns;
        _itemHeight = boardDimension.y / _rows;

        for (int i = 0; i < _items.Length; i++)
        {
            // Create a new Sprite GameObject
            GameObject newItem = Instantiate(itemPrefab, boardObj.transform);
            Image itemImage = newItem.GetComponent<Image>();
            itemImage.sprite = sprites[_items[i]];

            // Set the size and position of the sprite
            var pos = ItemPos2LocalPos(i);
            newItem.transform.localPosition = new Vector3(pos.x, pos.y, 0);
            newItem.transform.localScale = Vector3.one * Mathf.Min(_itemWidth, _itemHeight) / 100f; // Adjust scale as needed

            _objs[i] = newItem;
        }
    }
    public static void Shuffle<T>(T[] array, GameObject[] objs)
    {
        System.Random rng = new System.Random();

        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;

            if (objs != null)
            {
                GameObject obj = objs[k];
                objs[k] = objs[n];
                objs[n] = obj;

                Vector3 startPos = objs[k].transform.position;
                Vector3 endPos = objs[n].transform.position;

                objs[k].transform.position = endPos;
                objs[n].transform.position = startPos;
            }
        }
    }

    public void OnBtnMenu()
    {  
        GameManager.Goto("Menu");
    }
            
    public void OnBtnSetting()
    {
        GameManager.Goto("Setting");
    }

    public void OnBtnTryAgain()
    {
        btnTryAgain.gameObject.SetActive(false);

        DispLevel();
        DispMessage("REMEMBER\nTHE ORDER");

        _rows = GameManager.level + 1;
        PrepareItems();
        ArrangeSpritesInGrid();

        btnStart.gameObject.SetActive(true);
    }

    public void OnBtnStart()
    {
        DOTween.CompleteAll();
        DOTween.KillAll();

        congratulationObj.SetActive(false);
        btnStart.gameObject.SetActive(false);
        btnTryAgain.gameObject.SetActive(true);

        DispMessage("SWAP ITEMS\nGUESS ORDER");

        do
        {
            Shuffle(_items, _objs);
        } while (_items.SequenceEqual(_itemsOriginal));
    }

    public void OnBtnNext()
    {
        if (GameManager.level < levelNames.Length - 1)
        {
            GameManager.level++;
        }

        btnNext.gameObject.SetActive(false);
        congratulationObj.SetActive(false);

        OnBtnTryAgain();
    }

    private void DispLevel()
    {
        if (GameManager.level < levelNames.Length)
        {
            txtLevel.text = levelNames[GameManager.level];
        }
    }

    private void DispMessage(string msg)
    {
        txtMessage.text = msg;

        ItemArrangeEffect(txtMessage.transform);
    }

    private static void ItemArrangeEffect(Transform transform)
    {
        DG.Tweening.Sequence bounceSequence = DOTween.Sequence();
        bounceSequence.Append(transform.DOScale(1.2f, Random.Range(0.2f, 0.3f)).SetEase(Ease.OutQuad));
        bounceSequence.Append(transform.DOScale(1, Random.Range(0.2f, 0.3f)).SetEase(Ease.OutBounce));
    }

    private void SwapItems(int start, int end)
    {
        DOTween.CompleteAll();
        
        AudioManager.Instance.PlaySfx(sfxSwap);

        int temp = _items[start];
        _items[start] = _items[end];
        _items[end] = temp;

        GameObject obj = _objs[start];
        _objs[start] = _objs[end];
        _objs[end] = obj;

        Vector3 startPos = _objs[start].transform.position;
        Vector3 endPos = _objs[end].transform.position;

        const float swapTime = 0.3f;
        const float swapTimeVar = 0.2f;

        _objs[start].transform.DOMove(endPos, swapTime + Random.Range(0, swapTimeVar)).SetEase(Ease.InOutExpo);
        _objs[end].transform.DOMove(startPos, swapTime + Random.Range(0, swapTimeVar)).SetEase(Ease.InOutExpo);
    }

    private void CheckGame()
    {
        if (_items.SequenceEqual(_itemsOriginal)) {
            btnTryAgain.gameObject.SetActive(false);
            btnNext.gameObject.SetActive(true);
            congratulationObj.SetActive(true);

            AudioManager.Instance.PlaySfx(sfxCongratulation);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Get the start position of the touch/click
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardObj.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _startPosition
        );
    }

    // This method is called when the user ends touching (or releases click) the UI element
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!btnTryAgain.isActiveAndEnabled)
            return;

        // Get the end position of the touch/click
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardObj.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _endPosition
        );

        int select = LocalPos2ItemPos(_startPosition);
        if (select >= 0 && select < _items.Length) {
            // Handle the touch end event
            float distance = Vector2.Distance(_startPosition, _endPosition);
            if (distance > _itemWidth / 2) // threshold for a swipe
            {
                int x = select % _columns;
                int y = (int) (select / _columns);

                if (Mathf.Abs(_startPosition.x - _endPosition.x) > Mathf.Abs(_startPosition.y - _endPosition.y))
                {
                    if (_startPosition.x < _endPosition.x)
                        x++;
                    else
                        x--;
                }
                else
                {
                    if (_startPosition.y < _endPosition.y)
                        y++;
                    else
                        y--;
                }

                if (x >= 0 && x < _columns && y >= 0 && y < _rows)
                {
                    SwapItems(select, x + y *  _columns);
                    CheckGame();
                }
            }
        }

    }
}
