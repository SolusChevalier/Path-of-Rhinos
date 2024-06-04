using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileDev : MonoBehaviour
{
    #region FIELDS

    public TilePropertiesDev properties;
    private Vector3 _progression;
    private readonly Vector3 _speedIncrement = Vector3.one;
    private Vector3 _motionEquation;
    public Vector3 HoverAnchorLow = new Vector3(0, 0.2f, 0);
    public Vector3 HoverAnchorHigh = new Vector3(0, 0.4f, 0);
    public Vector3 HoverOverPos = new Vector3(0, 0.3f, 0);
    public SelectionState selectionState = SelectionState.Inert;
    public HoverState hoverState = HoverState.Static;
    private bool _Home = true;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()//starting properties
    {
        properties = new TilePropertiesDev(transform.position, transform.GetChild(1).transform);
        _motionEquation = new Vector3(Mathf.Floor(Random.Range(0, 3)), Mathf.Floor(Random.Range(0, 3)), Mathf.Floor(Random.Range(0, 3)));
        HoverAnchorLow += properties.StartPos;
        HoverAnchorHigh += properties.StartPos;
        HoverOverPos += properties.StartPos;

        selectionState = SelectionState.Inert;
        hoverState = HoverState.Static;
    }

    private void Update()
    {
        _progression += Time.deltaTime * _speedIncrement;
        Hover();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void SetSelectionSate(SelectionState state)//tile selection state will be implemented later
    {
        switch (state)
        {
            case SelectionState.Inert:
                selectionState = SelectionState.Inert;
                hoverState = HoverState.Static;
                break;

            case SelectionState.NotSelectable:
                selectionState = SelectionState.NotSelectable;
                hoverState = HoverState.Locked;
                break;

            case SelectionState.Selectable:
                selectionState = SelectionState.Selectable;
                hoverState = HoverState.Low;
                break;

            case SelectionState.Selected:
                selectionState = SelectionState.Selected;
                hoverState = HoverState.High;
                break;

            default:
                break;
        }
    }

    public void HoverOver()
    {
        if (hoverState == HoverState.Locked)
            return;
        if (hoverState == HoverState.Static)
        {
            hoverState = HoverState.HoverOver;
        }
    }

    public void EndHoverOver()
    {
        if (hoverState == HoverState.HoverOver)
        {
            hoverState = HoverState.Static;
        }
    }

    public void Hover()//tile hover state, will be implemented later
    {
        switch (hoverState)
        {
            case HoverState.Locked:
                if (_Home)
                    break;
                //transform.position = Vector3.Lerp(transform.position, properties.StartPos, Time.deltaTime * 5f);
                transform.position = properties.StartPos;
                _Home = true;
                break;

            case HoverState.Static:
                if (_Home)
                    break;
                //transform.position = Vector3.Lerp(transform.position, properties.StartPos, Time.deltaTime * 5f);
                transform.position = properties.StartPos;
                _Home = true;
                break;

            case HoverState.HoverOver:
                if (_Home)
                    _Home = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(
               HoverOverPos.x, HoverOverPos.y + TrigMotionEquations((int)_motionEquation.y, _progression.y, 0.15f, 0.05f), HoverOverPos.z),
               Time.deltaTime * 5f);
                break;

            case HoverState.Low:
                if (_Home)
                    _Home = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(
                HoverAnchorLow.x, HoverAnchorLow.y + TrigMotionEquations((int)_motionEquation.y, _progression.y, 0.15f, 0.05f), HoverAnchorLow.z),
                Time.deltaTime * 5f);
                break;

            case HoverState.High:
                if (_Home)
                    _Home = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(
                HoverAnchorHigh.x, HoverAnchorHigh.y + TrigMotionEquations((int)_motionEquation.y, _progression.y, 0.15f, 0.05f), HoverAnchorHigh.z),
                Time.deltaTime * 5f);
                break;

            default:
                break;
        }
    }

    public void Select()//sellects tile
    {
        if (selectionState == SelectionState.NotSelectable | selectionState == SelectionState.Inert)
            return;
        if (selectionState == SelectionState.Selected)
        {
            SetSelectionSate(SelectionState.Selectable);
            return;
        }
        if (selectionState == SelectionState.Selectable)
        {
            SetSelectionSate(SelectionState.Selected);
            return;
        }
    }

    //trig motion equations - they are random equations and are used to create a bobbing effect
    private float TrigMotionEquations(int equation, float progression, float frequency, float amplitude)
    {
        float result = 0f;
        switch (equation)
        {
            case 0:
                result = Mathf.Sin(1.8f * Mathf.Sin(Mathf.Cos(progression * 0.13f)) * Mathf.Cos((progression - 3f) * frequency)) * amplitude;
                break;

            case 1:
                result = Mathf.Sin((Mathf.Sin(progression * 0.8f) * 0.5f) * (Mathf.Cos(progression * 0.2f) * frequency)) * amplitude;
                break;

            case 2:
                result = (Mathf.Sin(progression * frequency)) * (Mathf.Sin((progression * 0.4f) * 0.3f)) * amplitude;
                break;
        }
        return result;
    }

    #endregion METHODS
}