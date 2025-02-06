using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using static SlotMap;
using System;


public class SlotGameSelection : MonoBehaviour
{
    [SerializeField]
    Button btnEmpty, btnBase, btnWall, btnTower, btnSpawnPoint;

    [Space(10)]

    [SerializeField]
    Image imgEmpty, imgBase, imgWall, imgTower, imgSpanwnPint;

    [SerializeField]
    Color colorSelected, colorBase;

    GameSlotContentType tipeSlotOfUse;


    private void Awake()
    {
        basicConfigurationInView();
    }

    private void basicConfigurationInView()
    {       
        btnEmpty.Select();
        asignateActionsOfButtons();
    }

    void asignateActionsOfButtons() 
    {
        btnEmpty.onClick.AddListener(() => pressBtnEmpty());
        btnBase.onClick.AddListener(() => pressBtnBase());
        btnWall.onClick.AddListener(() => pressBtnWall());
        btnTower.onClick.AddListener(() => pressBtnTower());
        btnSpawnPoint.onClick.AddListener(() => pressBtnSpawnPoint());
    }
    void changeColor(Color colorToChange) 
    {
        switch (tipeSlotOfUse) 
        {
            case GameSlotContentType.Empty :
                imgEmpty.color = colorToChange;
                break;

            case GameSlotContentType.Destination:
                imgBase.color = colorToChange;
                break;

            case GameSlotContentType.Wall:
                imgWall.color = colorToChange;
                break;

            case GameSlotContentType.Tower:
                imgTower.color = colorToChange;
                break;

            case GameSlotContentType.SpawnPoint:
                imgSpanwnPint.color = colorToChange;
                break;

        }
    }   

    void pressBtnEmpty() 
    {
        changueTypeOfSlot(GameSlotContentType.Empty);
    }

    void pressBtnBase()
    {
        changueTypeOfSlot(GameSlotContentType.Destination);
    }

    void pressBtnWall()
    {
        changueTypeOfSlot(GameSlotContentType.Wall);
    }

    void pressBtnTower()
    {
        changueTypeOfSlot(GameSlotContentType.Tower);
    }

    void pressBtnSpawnPoint()
    {        
        changueTypeOfSlot(GameSlotContentType.SpawnPoint);
    }

    void changueTypeOfSlot(GameSlotContentType newTypeSlot) 
    {
        changeColor(colorBase);
        tipeSlotOfUse = newTypeSlot;
        changeColor(colorSelected);
    }

    public GameSlotContentType gameSlotContentType { get { return tipeSlotOfUse;} }
}
