using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using GroceryStore.Constants;
using GroceryStore.Models;
using GroceryStore.Repositories;
using GroceryStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace GroceryStore.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class AdminOperationsController : Controller
{
    private readonly IUserOrderRepository _userOrderRepository;
    private readonly EmailService _emailService;
    private readonly SMSSender _SMSSender;
    private readonly ApplicationDbContext _context;
    public AdminOperationsController(IUserOrderRepository userOrderRepository, EmailService emailService, SMSSender sMSSender, ApplicationDbContext context)
    {
        _userOrderRepository = userOrderRepository;
        _emailService = emailService;
        _SMSSender = sMSSender;
        _context = context;

    }

    public async Task<IActionResult> AllOrders()
    {
        var orders = await _userOrderRepository.UserOrders(true);
        return View(orders);
    }
    [HttpGet]
    public IActionResult SendEmail()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _emailService.SendEmailAsync(model.ToEmail, model.Subject, model.Message);
            await _SMSSender.SendSmsAsync(model.MobileNumber, model.Message);


            ViewBag.Message = "Email sent successfully!";
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(EmailViewModel order)
    {

        await _SMSSender.SendSmsAsync(order.MobileNumber, order.Message);



        ViewBag.Message = "Message sent successfully!";

        return View();
    }





    public async Task<IActionResult> TogglePaymentStatus(int orderId)
    {
        try
        {
            await _userOrderRepository.TogglePaymentStatus(orderId);
        }
        catch (Exception ex)
        {
            // log exception here
        }
        return RedirectToAction(nameof(AllOrders));
    }

    public async Task<IActionResult> UpdateOrderStatus(int orderId)
    {
        var order = await _userOrderRepository.GetOrderById(orderId);
        if (order == null)
        {
            throw new InvalidOperationException($"Order with id:{orderId} does not found.");
        }
        var orderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus =>
        {
            return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = order.OrderStatusId == orderStatus.Id };
        });
        var data = new UpdateOrderStatusModel
        {
            OrderId = orderId,
            OrderStatusId = order.OrderStatusId,
            OrderStatusList = orderStatusList
        };
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                data.OrderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus =>
                {
                    return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = orderStatus.Id == data.OrderStatusId };
                });

                return View(data);
            }
            await _userOrderRepository.ChangeOrderStatus(data);
            TempData["msg"] = "Updated successfully";
        }
        catch (Exception ex)
        {
            // catch exception here
            TempData["msg"] = "Something went wrong";
        }
        return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
    }


    public async Task<IActionResult> Dashboard()
    {
        var orders = await _userOrderRepository.UserOrders(true);
        return View(orders);
    }

    public List<object> GetOrders()
    {
        List<object> data = new List<object>();
        //List<object> orders = new List<object>();
        
        List<object> final = new List<object>();

        var order = _context.Orders.Include(x => x.OrderDetail).GroupBy(x => x.CreateDate);
        var dates = order.Select(x => x.Select(x => x.CreateDate.ToString("MMM")));
        var count = 0;
        var count1 = 0;
        var count3 = 0;
        var count4 = 0;
        var count5 = 0;
        foreach (var items in order)
        {
            var month = items.Select(x => x.CreateDate.Month).FirstOrDefault();
            if (month==1)
            {
                foreach (var item in items.Select(x =>x.OrderDetail))
    {

                    //orders.Add(item.Select(x => x.UnitPrice * x.Quantity).Sum());
                    count = (int)(count + item.Select(x => x.UnitPrice * x.Quantity).Sum());
    }

            }
            
            if (month == 2)
            {
                foreach (var item in items.Select(x => x.OrderDetail))
                {

                    //final.Add(item.Select(x => x.UnitPrice * x.Quantity).Sum());
                    count1 = (int)(count1+item.Select(x => x.UnitPrice * x.Quantity).Sum());
                }

            }
            if (month == 3)
            {
                foreach (var item in items.Select(x => x.OrderDetail))
                {

                    //final.Add(item.Select(x => x.UnitPrice * x.Quantity).Sum());
                    count3 = (int)(count3 + item.Select(x => x.UnitPrice * x.Quantity).Sum());
                }

            }
            if (month == 4)
            {
                foreach (var item in items.Select(x => x.OrderDetail))
                {

                    //final.Add(item.Select(x => x.UnitPrice * x.Quantity).Sum());
                    count4 = (int)(count4 + item.Select(x => x.UnitPrice * x.Quantity).Sum());
                }

            }
            if (month == 5)
            {
                foreach (var item in items.Select(x => x.OrderDetail))
                {

                    //final.Add(item.Select(x => x.UnitPrice * x.Quantity).Sum());
                    count5 = (int)(count5 + item.Select(x => x.UnitPrice * x.Quantity).Sum());
                }

            }
        }

        final.Add(count);
        final.Add(count1);
        final.Add(count3);
        final.Add(count4);
        final.Add(count5);
        
        data.Add(final);
        return data;
    }

        
       


    }

