﻿using ProProperty.DAL;
using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProProperty.Controllers
{
    public class PropertyController : Controller
    {
        private PropertyGateway propertyDataGateway = new PropertyGateway();
        private TownGateway townDataGateway = new TownGateway();
        private AgentGateway agentGateway = new AgentGateway();
        private static List<PropertyWithPremises> propertyList = new List<PropertyWithPremises>();

        // GET: Property
        public ActionResult Index()
        {
            return View();
        }

        // GET: Property/Details/5
        public ActionResult PropertyDetails(int id)
        {
            foreach(PropertyWithPremises p in propertyList)
            {
                if (p.property.propertyID == id)
                {
                    return View(p);
                }
            }

            return RedirectToAction("Index", "Search");
        }

        public ActionResult PropertyInformation(int id)
        {
            Property propertyObj = propertyDataGateway.SelectById(id);
            int townID = propertyObj.HDBTown;
            Town townName = townDataGateway.SelectById(townID);
            
            if (propertyObj != null)
            {
                Agent agt = agentGateway.SelectById(propertyObj.agent_id);
                ViewBag.AgentName = agt.agent_name;
                ViewBag.AgentContactNumber = agt.agent_contact_number;
                ViewBag.AgentEmail = agt.agent_email;
                ViewBag.AgentImage = agt.agent_image;

                ViewBag.Town_Name = townName.town_name; //get town name and store in ViewBag
                ViewBag.Property_Room_Type = propertyObj.GetRoomType().ToString() + "-room"; //get room type and store in ViewBag
                ViewBag.CurrentPrice = propertyObj.asking;

                return View(propertyObj);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
            
        }

        // Controller public methods
        public static void addProperty(PropertyWithPremises property)
        {
            propertyList.Add(property);
        }

        public static void clearListProperty()
        {
            propertyList.Clear();
        }

        public static IEnumerable<PropertyWithPremises> getAllProperties()
        {
            return propertyList;
        }
    }
}