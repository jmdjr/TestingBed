using DigitalRuby.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerticleSelector : MonoBehaviour {

    public string startingItem = "";
    //public string startingItem = Main.Instance.Player.ActiveFaction;

    public float MovingTimeout = 0.5f;
    public RectTransform RectArea;
    public ScrollerSelectorItem scrollItem;

    public ScrollRect ScrollingArea;
    private List<ScrollerSelectorItem> Items = new List<ScrollerSelectorItem>();

    public bool isDragging = false;
    //public float NormalizedViewPosition;
    private int SI;

    public string SelectedItem { get; private set; }

    public void SelectItem(string SelectItem)
    {
        if (Items == null || Items.Count == 0) return;

        ScrollerSelectorItem item = Items.FirstOrDefault(i => i.Data == SelectItem);

        if (item == null) return;

        SelectItemByName(SelectItem);
        ScrollingArea.normalizedPosition = new Vector2(0f, 1.0f - (Items.IndexOf(item) / (1.0f * (Items.Count() - 1))));
    }

    // Use this for initialization
    public IEnumerator Start ()
    {
        SelectedItem = startingItem;
        ScrollingArea.onValueChanged.RemoveAllListeners();
        ScrollingArea.onValueChanged.AddListener(ScrollingAreaMoved);

        yield return new WaitForEndOfFrame();
    }

    private void ScrollingAreaMoved(Vector2 v2)
    {
        isDragging = Input.GetMouseButton(0);
    }

    public void SelectItemByName(string itemName)
    {
        SelectedItem = itemName;
    }

    public void SetScrollItemList(List<string> ItemNames, List<string> ItemData, List<Sprite> graphics = null)
    {
        // The developement lists are inconsistent, can't build things...
        // can't make or hide item...
        // maybe throw an error here?
        if (ItemNames.Count != ItemData.Count || !scrollItem || !RectArea || (graphics != null && graphics.Count != ItemNames.Count)) return;

        for (int index = 0; index < ItemData.Count; ++index)
        {
            string name = ItemNames[index], data = ItemData[index];
            Sprite sprite = null;

            if (graphics != null) sprite = graphics[index];

            ScrollerSelectorItem one = BuildOne(name, data, sprite);
            Items.Add(one);
        }
    }

    private ScrollerSelectorItem BuildOne(string name, string data, Sprite image = null)
    {
        var one = Instantiate(scrollItem, RectArea).Set(name, data, image);
        one.gameObject.SetActive(true);
        return one;
    }

    private void SelectFromLastPosition()
    {
        Vector2 NVP = ScrollingArea.normalizedPosition;
        StartCoroutine(StartSlideBack(NVP));
    }

    private Vector2 NewNormalPoint(Vector2 NVP)
    {
        int total = Items.Count;
        float nvp = NVP.y < 0 ? 0 : NVP.y;
        float NVM = nvp * (total-1);

        // round to nearest index position
        SI = Mathf.RoundToInt(NVM);

        // calculate new Normalized Verticle Point
        NVM = SI / (total - 1.0f);

        // invert index
        SI = (total - 1) - SI;

        if (SI > -1 && Items.Count > SI)
        {
            SelectItemByName(Items[SI].Data);
        }
        else
        {
            Debug.Log("SI out of Bounds");
        }

        return new Vector2(0.0f, NVM);
    }

    IEnumerator StartSlideBack(Vector2 NVP)
    {
        Vector2 CalculatedPosition = NewNormalPoint(NVP);
        Vector2Tween tween = TweenFactory.Tween(null, NVP, CalculatedPosition, 0.3f, null, null);

        while (tween.CurrentProgress < 1)
        {
            ScrollingArea.normalizedPosition = tween.CurrentValue;
            yield return new WaitForEndOfFrame();
        }

        ScrollingArea.normalizedPosition = tween.EndValue;
    }

    public float curTime = 0.0f;
    public bool endMotion = false;

    // Update is called once per frame
    void Update () {
        //NormalizedViewPosition = ScrollingArea.verticalNormalizedPosition;

        isDragging = Input.anyKey;

        if (!isDragging)
        {
            if(curTime <= MovingTimeout)
            {
                curTime += Time.deltaTime;
            }
        }
        else
        {
            endMotion = false;
            curTime = 0;
        }

        if (curTime >= MovingTimeout && !endMotion)
        {
            endMotion = true;
            SelectFromLastPosition();
        }
	}
}
