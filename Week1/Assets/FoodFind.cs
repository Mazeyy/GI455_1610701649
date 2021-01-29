using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodFind : MonoBehaviour
{
    public List<string> nameFood;
    public InputField input_field;
    public List<Text> FoodName;
    public GameObject output;

    public void Start()
    {
        for(int i = 0; i <= nameFood.Count - 1; i++)
        {
            FoodName[i].text = nameFood[i].ToString();
            //Debug.Log("Set on Start UP");
        }
    }

    public void FindName(string FoodName)
    {
        if (nameFood.Contains(input_field.text))
        {
            FoodName = input_field.text;
            output.GetComponent<Text>().text = "[ " + "<b><i>" + "<color=lime>" + FoodName + "</color>" + "</i></b>" + " ]" + " is found.";
            //Debug.Log("Found");
        }
        else
        {
            FoodName = input_field.text;
            output.GetComponent<Text>().text = "[ " + "<b><i>" + "<color=red>" + FoodName + "</color>" + "</i></b>" + " ]" + " is not found.";
            //Debug.Log("Not Found");
        }
    }
}
