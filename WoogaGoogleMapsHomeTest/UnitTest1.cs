using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support;
using NUnit.Framework.Internal.Commands;

namespace WoogaGoogleMapsHomeTest
{

    public class Tests
    {
        IWebDriver webDriver;
        //choose web driver here
        string pickBrowser = "chrome";
        int count = 1;
        
        [OneTimeSetUp]
        public void StartGoogleMaps()
        {
            CrossPlaform(pickBrowser);
        }

        public void SearchForLocation(string location,string text)
        {
            IWebElement searchIcon;
            IWebElement searchBar;

            //Assigning and finding the xpaths for the searchbar and search button.
            searchBar = webDriver.FindElement(By.XPath("//input[@id=\"searchboxinput\"]"));
            searchIcon = webDriver.FindElement(By.XPath("//button[@id=\"searchbox-searchbutton\"]"));

            searchBar.SendKeys(location);
            searchIcon.Click();
            Task.Delay(5000).Wait();
            /*assert that an element on the page is true to make sure the page has changed
            Assert.Equals had issues with reading the text directly so the use of String,Equals
            helped to avoid the issue
            xpath allows for use with large veriety of elements on the page to look for the text
            as shows by finding place locations and info text with the same xpath */
            Assert.IsTrue(String.Equals(webDriver.FindElement(By.XPath("//*[contains(text(),'" + text + "')]")).Text, text));
            searchBar.Clear();
        }

        public void TakeScreenShot()
        {
            //takes a screenshot of each google maps search, names its TestScrenshot[broswer]-[count] 
            ((ITakesScreenshot)webDriver).GetScreenshot().SaveAsFile("TestScreenShot" + pickBrowser + "-" + count + ".png", ScreenshotImageFormat.Png);
            count++;
        }

        public void CheckForCookieConfirmation()
        {
            IWebElement acceptButton;

            //checks if browser vierfication exists for new instances of all broswers
            if (webDriver.FindElements(By.XPath("//button[@aria-label='Accept all']")).Count > 0)
            {
                acceptButton = webDriver.FindElement(By.XPath("//button[@aria-label='Accept all']"));
                Task.Delay(3000).Wait();
                acceptButton.Click();
                Task.Delay(3000).Wait();
            }
        }

        [Test]
        public void SearchForMapLocations()
        {
            webDriver.Url = "https://www.google.com/maps";

            //checks if need to accept cookies or not
            CheckForCookieConfirmation();

            //searching for 4 locations on google maps
            SearchForLocation("Strandhill, Co. Sligo, Ireland", "Strandhill or occasionally Larass is a coastal town and townland on the Coolera Peninsula in County Sligo, Ireland. As of 2016, the population was 1,753, an increase of 10% from the 2011 Census. The old name appears to be Ros Dragnige.");
            TakeScreenShot();
            SearchForLocation("Wooga, Saarbrücker Straße, Berlin, Duitsland", "Saarbrücker Str. 38, 10405 Berlin, Germany");
            TakeScreenShot();
            SearchForLocation("Olympic Park, Olympic-ro, Songpa District, Seoel, Zuid-Korea", "424 Olympic-ro, Songpa-gu, Seoul, South Korea");
            TakeScreenShot();
            SearchForLocation("Laxey Wheel, Laxey, Isle of Man", "Laxey, Isle of Man");
            TakeScreenShot();

            Assert.Pass();
        }

        public void CrossPlaform(string browser)
        {
            //swtich statment to set up choosen webdriver
            switch (browser.ToLower())
            {
                case "firefox":
                    webDriver = new FirefoxDriver(".");
                    break;
                case "ie":
                    webDriver = new InternetExplorerDriver(".");
                    break;
                case "safari":
                    webDriver = new SafariDriver(".");
                    break;
                case "edge":
                    webDriver = new EdgeDriver(".");
                    break;
                case "chrome":
                    webDriver = new ChromeDriver(".");
                    break;
                default:
                    break;
            }
        }

        [OneTimeTearDown]
        public void CloseSession()
        {
            //will always close when every test is finished
            webDriver.Close();
        }

    }
}