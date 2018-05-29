﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextElement : PageElement {

    private Text text;

    public TextElement()
    {
        //Set any default fields
        text.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        text.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        text.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);

        //Set fields that user will not be allowed to change
    }

    public void setText(string newText)
    {
        text.text = newText;
    }

    public string getText()
    {
        return text.text;
    }

    public void setFont(Font newFont)
    {
        text.font = newFont;
    }

    public void setFontSize(int newSize)
    {
        text.fontSize = newSize;
    }

    public void setFontStyle(FontStyle style)
    {
        text.fontStyle = style;
    }

    public new string toString()
    {
        return "TextElement that reads: " + text.text + " " + base.toString();
    }
}
