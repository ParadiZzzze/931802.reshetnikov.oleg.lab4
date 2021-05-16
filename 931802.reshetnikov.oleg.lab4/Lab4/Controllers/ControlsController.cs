using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class ControlsController : Controller
    {
        public IActionResult Controls()
        {
            return View();
        }
      
        [HttpGet]
        public IActionResult TextBox()
        {
            ViewData["Title"] = "TextBox";
            return View();
        }
        [HttpPost]
        public IActionResult TextBox(string Text)
        {
            return SetViewBagAndDataRes("TextBox", "Text", Text);
        }
       
        [HttpGet]
        public IActionResult TextArea()
        {
            ViewData["Title"] = "TextArea";
            return View();
        }
        [HttpPost]
        public IActionResult TextArea(string Text)
        {
            return SetViewBagAndDataRes("TextArea", "Text", Text);
        }
       
        [HttpGet]
        public IActionResult Radio()
        {
            ViewData["Title"] = "Radio";
            return View();
        }
        [HttpPost]
        public IActionResult Radio(string Radio)
        {
            return SetViewBagAndDataRes("CheckBox", "Month", Radio);
        }

        [HttpGet]
        public IActionResult CheckBox()
        {
            ViewData["Title"] = "CheckBox";
            return View();
        }
        [HttpPost]
        public IActionResult CheckBox(string Text)
        {
            return SetViewBagAndDataRes("radio", "IsSelected", Text);
        }

        [HttpGet]
        public IActionResult DropDownList()
        {
            ViewData["Title"] = "DropDownList";
            SetViewBagMonths();
            return View();
        }
        [HttpPost]
        public IActionResult DropDownList(string Text)
        {
            SetViewBagMonths();
            return SetViewBagAndDataRes("DropDownList", "Month", Text);
        }

        [HttpGet]
        public IActionResult ListBox()
        {

            SetViewBagMonths();
            return View();
        }
        [HttpPost]
        public IActionResult ListBox(string Text)
        {
            return SetViewBagAndDataRes("ListBox", "Month", Text);
        }

        private IActionResult SetViewBagAndDataRes(string Title, string Type, string Value)
        {
            ViewData["Title"] = Title;
            ViewBag.ResultType = Type;
            ViewBag.ResultValue = Value;
            return View("~/Views/Controls/Result.cshtml");
        }
        private void SetViewBagMonths()
        {
            string[] months_str_mass = { "January", "February", "March", "April",
                "May", "June", "July", "August", "September", "October", "November", "December" };
            SelectList months_SList = new SelectList(months_str_mass, "month");
            ViewBag.Months = months_SList;
        }
        
    }
}