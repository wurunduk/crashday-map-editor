using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGuiControls 
{
	public static bool DrawIntSlider(Rect position, ref int currentNumber)
	{
		string fieldText = GUI.TextField(new Rect(position.x + 30, position.y, 85, 25), currentNumber.ToString());

		int answer;

		//if we completely removed everything from the field, change it to 0
		if (String.IsNullOrEmpty(fieldText))
			fieldText = "0";

		if (Int32.TryParse(fieldText, out answer))
		{
			if (currentNumber != answer)
			{
				currentNumber = answer;
				return true;
			}

			currentNumber = answer;
		}

		if (GUI.Button(new Rect(position.x, position.y, 25, 25), "-"))
		{
			currentNumber -= 1;
			return true;
		}

		if (GUI.Button(new Rect(position.x + 120, position.y, 25, 25), "+"))
		{
			currentNumber += 1;
			return true;
		}

		return false;
	}

	public static bool DrawFloatSlider(Rect position, ref float currentNumber, float step = 10.0f)
	{
		string fieldText = GUI.TextField(new Rect(position.x + 30, position.y, 85, 25), currentNumber.ToString("0.00"));

		float answer;

		//if we completely removed everything from the field, change it to 0
		if (String.IsNullOrEmpty(fieldText))
			fieldText = "1";

		if (float.TryParse(fieldText, out answer))
		{
			if (currentNumber != answer)
			{
				currentNumber = answer;
				return true;
			}

			currentNumber = answer;
		}

		if (GUI.Button(new Rect(position.x, position.y, 25, 25), "-"))
		{
			currentNumber -= step;
			return true;
		}

		if (GUI.Button(new Rect(position.x + 120, position.y, 25, 25), "+"))
		{
			currentNumber += step;
			return true;
		}

		return false;
	}

	public static string DrawNamedTextField(Rect position, string name, string currentString)
	{
		GUI.Label(position, name);
		position.x += 100.0f;
		position.width -= 100.0f;
		return GUI.TextField(position, currentString);
	}
}
