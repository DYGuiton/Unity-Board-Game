using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAssets : MonoBehaviour {

    [SerializeField]
    public int wood { get; private set; } = 0;
    [SerializeField]
    public int food { get; private set; } = 0;
    [SerializeField]
    public int joy { get; private set; } = 0;

    #region Increments

    public int IncrementWood(int increment) {
        while(increment !=0) {
            wood++;
            increment--;
        }

        return wood;
    }

    public int IncrementFood(int increment) {
        while (increment != 0) {
            food++;
            increment--;
        }

        return food;
    }

    public int IncrementJoy(int increment) {
        while (increment != 0) {
            joy++;
            increment--;
        }

        return joy;
    }

    #endregion

    #region Decrements

    public int DecrementWood(int decrement) {
        while(decrement != 0) {
            if(wood > 0) {
                wood--;
            }
            decrement--;
        }
        return wood;
    }

    public int DecrementFood(int decrement) {
        while (decrement != 0) {
            if (food > 0) {
                food--;
            }
            decrement--;
        }
        return food;
    }

    public int DecrementJoy(int decrement) {
        while (decrement != 0) {
            if (joy > 0) {
                joy--;
            }
            decrement--;
        }
        return joy;
    }

    #endregion

}