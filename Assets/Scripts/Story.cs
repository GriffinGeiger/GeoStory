using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story
{
    private Dictionary<string, Page> pages = new Dictionary<string, Page>();
    private Page currentPage;


    public void changePage(Page nextPage)
    {
        try{
            currentPage.setVisible(false);
        }catch(System.NullReferenceException nre) {/*do nothing since this just means there was no current page*/}
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

    public void removePage(string name)
    {
        pages.Remove(name);
    }

    public bool pageNameExists(string name)
    {
        return pages.ContainsKey(name);
    }
}
