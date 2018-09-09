using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts;

[Serializable()]
public class Story : ISerializable
{
    private Dictionary<string, Page> pages = new Dictionary<string, Page>();
    public Page currentPage;
    public string firstPageName;
    public string name { get; set; } 

    
    public Story(){}

    //Deserialization constructor
    public Story(SerializationInfo info, StreamingContext ctxt)
    {
        pages = (Dictionary<string, Page>)info.GetValue("pages", typeof(Dictionary<string, Page>));
        currentPage = (Page)info.GetValue("currentPage", typeof(Page));
        name = (string)info.GetValue("name", typeof(string));
    }

    public void changePage(Page nextPage)
    {
        try{
            currentPage.setVisible(false);
        }catch(System.NullReferenceException) {/*do nothing since this just means there was no current page*/}
        currentPage = nextPage;
        currentPage.setVisible(true);
    }

    public void setCurrentPage(string pageName)
    {
        if (pageNameExists(pageName))
            changePage(pages[pageName]);
        else
            throw new KeyNotFoundException("There is no page named " + pageName + " in this story");
    }

    public Page getCurrentPage()
    {
        return currentPage;
    }

    public void addPage(Page newPage)
    {
        pages.Add(newPage.getName(), newPage);

    }

    public Page getPage(string pageName)
    {
        //pagenames that have copies with tags (1) are screwing this up
        return pages[pageName];
    }

    public Page[] getPages()
    {
        Dictionary<string, Page>.ValueCollection valueColl = pages.Values;
        Page[] pageArray = new Page[pages.Count];
        valueColl.CopyTo(pageArray,0);
        return pageArray;
    }

    public void removePage(string name)
    {
        Page page = pages[name];
        //remove any references to this page
        ConnectionsLibrary.removeConnectionsTo(this, page);
        pages.Remove(name);
    }

    public bool pageNameExists(string name)
    {
        return pages.ContainsKey(name);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("pages", pages);
        info.AddValue("currentPage", currentPage);
        info.AddValue("name", name);
    }
}
