# 在ASP.NET MVC中使用Web API和EntityFramework构建应用程序 #
最近做了一个项目技术预研：在ASP.NET MVC框架中使用Web API和EntityFramework，构建一个基础的架构，并在此基础上实现基本的CRUD应用。

以下是详细的步骤。

## 第一步 ##
在数据库中创建一张数据表，表名为Customer，见下图：

![](https://i.imgur.com/w3HHT77.jpg)
## 第二步 ##


- 打开 Visual Studio，新建项目。


- 选择'ASP.NET Web Application',命名为'WebApi'。
![](https://i.imgur.com/wL85bvR.jpg)


- 我们要创建一个'Web API'，在模板中选择'Web API',点击'确定'。
![](https://i.imgur.com/mIAng6J.jpg)
## 第三步 ##
接下来我要添加一个类。


- 右键点击这个 web api项目，添加一个'ADO.NET Entity Data Model'。
![](https://i.imgur.com/aINxcEF.jpg)


- 添加'EF Designer from database',点击'下一步'。
![](https://i.imgur.com/ivpeYT1.gif)


- 在配置窗口中添加新连接，输入服务器名称，选择数据库，点击'确定'。
![](https://i.imgur.com/iNkV6ak.jpg)


- 点击'下一步'，选择要关联的数据表。
![](https://i.imgur.com/sbu0Zxu.gif)


- 点击'完成'。
## 第四步 ##
现在的工作就是要为Wep Api添加控制器了。


- 右键选择'controllers'文件夹。
- 
![](https://i.imgur.com/ZznxUmp.jpg)


- 选择'MVC5 Controller with views, using Entity Framework'。
![](https://i.imgur.com/385ymWb.jpg)


- 点击'添加'。

- 选择数据模型类Customer，选择Data context类文件，本例中为SystemTestEntity。

- 设置控制器名称为CustomersController，点击'添加'。
![](https://i.imgur.com/0X6L1DJ.gif)
上述操作完成后，CustomerController.cs会自动生成。

**CustomerController.cs**
```
using System.Data.Entity;  
using System.Data.Entity.Infrastructure;  
using System.Linq;  
using System.Net;  
using System.Web.Http;  
using System.Web.Http.Description;  
using CRUDWebApiExam.Models;  
  
namespace CRUDWebApiExam.Controllers  
{  
    public class CustomersController : ApiController  
    {  
        private SystemTestEntities db = new SystemTestEntities();  
  
        // GET: api/Customers  
        public IQueryable<Customer> GetCustomer()  
        {  
            return db.Customer;  
        }  
  
        // GET: api/Customers/5  
        [ResponseType(typeof(Customer))]  
        public IHttpActionResult GetCustomer(int id)  
        {  
            Customer customer = db.Customer.Find(id);  
            if (customer == null)  
            {  
                return NotFound();  
            }  
            return Ok(customer);  
        }  
  
        // PUT: api/Customers/5  
        [ResponseType(typeof(void))]  
        public IHttpActionResult PutCustomer(int id, Customer customer)  
        {  
            if (!ModelState.IsValid)  
            {  
                return BadRequest(ModelState);  
            }  
            if (id != customer.CustomerId)  
            {  
                return BadRequest();  
            }  
            db.Entry(customer).State = EntityState.Modified;  
            try  
            {  
                db.SaveChanges();  
            }  
            catch (DbUpdateConcurrencyException)  
            {  
                if (!CustomerExists(id))  
                {  
                    return NotFound();  
                }  
                else  
                {  
                    throw;  
                }  
            }  
            return StatusCode(HttpStatusCode.NoContent);  
        }  
  
        // POST: api/Customers  
        [ResponseType(typeof(Customer))]  
        public IHttpActionResult PostCustomer(Customer customer)  
        {  
            if (!ModelState.IsValid)  
            {  
                return BadRequest(ModelState);  
            }  
            db.Customer.Add(customer);  
            db.SaveChanges();   
            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);  
        }  
  
        // DELETE: api/Customers/5  
        [ResponseType(typeof(Customer))]  
        public IHttpActionResult DeleteCustomer(int id)  
        {  
            Customer customer = db.Customer.Find(id);  
            if (customer == null)  
            {  
                return NotFound();  
            }  
            db.Customer.Remove(customer);  
            db.SaveChanges();  
            return Ok(customer);  
        }  
  
        protected override void Dispose(bool disposing)  
        {  
            if (disposing)  
            {  
                db.Dispose();  
            }  
            base.Dispose(disposing);  
        }  
  
        private bool CustomerExists(int id)  
        {  
            return db.Customer.Count(e => e.CustomerId == id) > 0;  
        }  
    }  
}  
```
运行这个 Web Api项目，你会看到下面的结果
![](https://i.imgur.com/5rxmuuV.jpg)

## 第五步 ##

到此为止，web api已经构建成功了。接下来我们需要为这个web api服务添加消费对象了，这个消费对象就是本文要涉及的MVC项目。


- 首先要添加一个model类，我们可以直接采用上文中的Customer类。右键点击models文件夹，添加类并命名。本例中，我直接命名为Customer.cs。
**Customer.cs**

```
using System;  
using System.ComponentModel.DataAnnotations;  
  
namespace MVCPersatantion.Models  
{  
    public class Customer  
    {  
        [Display(Name = "CustomerId")]  
        public int CustomerId { get; set; }  
        [Display(Name = "Name")]  
        public string Name { get; set; }  
        [Display(Name = "Address")]  
        public string Address { get; set; }  
        [Display(Name = "MobileNo")]  
        public string MobileNo { get; set; }  
        [Display(Name = "Birthdate")]  
        [DataType(DataType.Date)]  
        public DateTime Birthdate { get; set; }  
        [Display(Name = "EmailId")]  
        public string EmailId { get; set; }          
    }   
}  
```
- customer类添加完成后，接下来需要添加一个ViewModel文件夹，并在里面新建一个CustomerViewModel.cs文件。
**CustomerViewModel.cs**

```
using MVCPersatantion.Models;  
  
namespace MVCPersatantion.ViewModel  
{  
    public class CustomerViewModel  
    {  
        public Customer customer { get; set; }  
    }  
}
``` 
## 第六步 ##

我已经添加了一个消费web api服务的类，现在我还需要添加一个Client类。


- 右键点击models文件夹，添加类文件CustomerClient.cs。
**CustomerClient.cs**

```
using System;  
using System.Collections.Generic;  
using System.Net.Http;  
using System.Net.Http.Headers;  
  
namespace MVCPersatantion.Models  
{  
    public class CustomerClient  
    {  
        private string Base_URL = "http://localhost:30110/api/";  
  
        public IEnumerable<Customer> findAll()  
        {  
            try  
            {  
                HttpClient client = new HttpClient();  
                client.BaseAddress = new Uri(Base_URL);  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                HttpResponseMessage response = client.GetAsync("customers").Result;  
  
                if (response.IsSuccessStatusCode)  
                    return response.Content.ReadAsAsync<IEnumerable<Customer>>().Result;  
                return null;  
            }  
            catch  
            {  
                return null;  
            }  
        }  
        public Customer find(int id)  
        {  
            try  
            {  
                HttpClient client = new HttpClient();  
                client.BaseAddress = new Uri(Base_URL);  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                HttpResponseMessage response = client.GetAsync("customers/" + id).Result;  
  
                if (response.IsSuccessStatusCode)  
                    return response.Content.ReadAsAsync<Customer>().Result;  
                return null;  
            }  
            catch  
            {  
                return null;  
            }  
        }  
        public bool Create(Customer customer)  
        {  
            try  
            {  
                HttpClient client = new HttpClient();  
                client.BaseAddress = new Uri(Base_URL);  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                HttpResponseMessage response = client.PostAsJsonAsync("customers", customer).Result;  
                return response.IsSuccessStatusCode;  
            }  
            catch  
            {  
                return false;  
            }  
        }  
        public bool Edit(Customer customer)  
        {  
            try  
            {  
                HttpClient client = new HttpClient();  
                client.BaseAddress = new Uri(Base_URL);  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                HttpResponseMessage response = client.PutAsJsonAsync("customers/" + customer.CustomerId, customer).Result;  
                return response.IsSuccessStatusCode;  
            }  
            catch  
            {  
                return false;  
            }  
        }  
        public bool Delete(int id)  
        {  
            try  
            {  
                HttpClient client = new HttpClient();  
                client.BaseAddress = new Uri(Base_URL);  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                HttpResponseMessage response = client.DeleteAsync("customers/" + id).Result;  
                return response.IsSuccessStatusCode;  
            }  
            catch  
            {  
                return false;  
            }  
        }  
    }  
}
```  
 细心的朋友们可能已经发现，我的代码中有一些重复代码。实际应用中我们可以抽取成公用代码，这样任何地方都能够使用了。这里为了清楚的阐述过程，所以写了一些重复的代码。

我们看第一行`private string Base_URL = "http://localhost:50985/api/"`，这个Base_URL是web api的服务地址。

接下来就可以编写Insert、Update、Delete、Select的业务逻辑了。

## 第七步 ##

右键点击Controllers文件夹，添加一个名为Customer控制器，控制器中添加服务客户端调用的方法。

```
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Web;  
using System.Web.Mvc;  
using MVCPersatantion.Models;  
using MVCPersatantion.ViewModel;  
  
namespace MVCPersatantion.Controllers  
{  
    public class CustomerController : Controller  
    {  
        // GET: Customer  
        public ActionResult Index()  
        {  
            CustomerClient CC = new CustomerClient();  
            ViewBag.listCustomers = CC.findAll();   
            return View();  
        }  
        [HttpGet]  
        public ActionResult Create()  
        {  
            return View("Create");  
        }  
        [HttpPost]  
        public ActionResult Create(CustomerViewModel cvm)  
        {  
            CustomerClient CC = new CustomerClient();  
            CC.Create(cvm.customer);  
            return RedirectToAction("Index");  
        }  
         
        public ActionResult Delete(int id)  
        {  
            CustomerClient CC = new CustomerClient();  
            CC.Delete(id);  
            return RedirectToAction("Index");  
        }  
        [HttpGet]  
        public ActionResult Edit(int id)  
        {  
            CustomerClient CC = new CustomerClient();  
            CustomerViewModel CVM = new CustomerViewModel();  
            CVM.customer = CC.find(id);  
            return View("Edit",CVM);  
        }  
        [HttpPost]  
        public ActionResult Edit(CustomerViewModel CVM)  
        {  
            CustomerClient CC = new CustomerClient();  
            CC.Edit(CVM.customer);  
            return RedirectToAction("Index");  
        }  
    }  
}  
```
## 第八步 ##

- 创建视图页面Index.cshtml。


- 在Index页面中添加代码。
**Index.cshtml**

```
@{
    ViewBag.Title = "Index";
}

<h2>Index</h2>

<div align="center">
    @Html.ActionLink("Create", "Create", "Customer", new { area = "" }, null)
    <br /><br />
    <table cellpadding="2"
           class="table"
           cellspacing="2"
           border="1">
        <tr class="btn-primary">

            <th>
                CustomerId
            </th>
            <th> Name</th>
            <th> Address</th>
            <th> MobileNo</th>
            <th> Birthdate</th>
            <th> EmailId</th>
            <th> Action</th>
        </tr>
        @foreach (var Cust in ViewBag.listCustomers)
        {
            <tr class="btn-success">
                <td>
                    @Cust.CustomerId
                </td>
                <td>
                    @Cust.Name
                </td>
                <td>
                    @Cust.Address
                </td>
                <td>
                    @Cust.MobileNo
                </td>
                <td>
                    @Cust.Birthdate.ToString("dd/MM/yyyy")
                </td>
                <td>
                    @Cust.EmailId
                </td>
                <td>
                    <a onclick="return confirm('确定要删除这条数据?')"
                       href="@Url.Action("Delete","Customer",new {id= Cust.CustomerId})">Delete</a> ||
                    <a href="@Url.Action("Edit","Customer",new {id= Cust.CustomerId})">Edit</a>
                   @*@Html.ActionLink("Edit", "Edit", "Customer", new { id = Cust.CustomerId })*@
                </td>

            </tr>
        }
    </table>
</div>
``` 


- 添加新增数据模块Create。


- 在Customer控制器中添加create方法，在view中增加Create.cshtml。

**Create.cshtml**

```
@model WebApiMVC.ViewModel.CustomerViewModel

@{
    ViewBag.Title = "Create";
}

<h2>Create</h2>

<div align = "center" >
@using (Html.BeginForm("create", "Customer", FormMethod.Post))
{
    <table class="table">
        <tr>
            <td>
                @Html.LabelFor(model => model.customer.Name)
            </td>

            <td>
                @Html.TextBoxFor(model => model.customer.Name)
            </td>
        </tr>
        <tr>
            <td>
                @Html.LabelFor(model => model.customer.Address)
            </td>

            <td>
                @Html.TextBoxFor(model => model.customer.Address)
            </td>
        </tr>
        <tr>
            <td>
                @Html.LabelFor(model => model.customer.MobileNo)
            </td>

            <td>
                @Html.TextBoxFor(model => model.customer.MobileNo)
            </td>
        </tr>
        <tr>
            <td>
                @Html.LabelFor(model => model.customer.Birthdate)
            </td>

            <td>
                @Html.TextBoxFor(model => model.customer.Birthdate)
            </td>
        </tr>
        <tr>
            <td>
                @Html.LabelFor(model => model.customer.EmailId)
            </td>

            <td>
                @Html.TextBoxFor(model => model.customer.EmailId)
            </td>
        </tr>
        <tr>
            <td></td>

            <td>
                <input type="submit"  value="保存" />
            </td>
        </tr>
    </table>
}
    </div>

```


- 添加修改数据模块Edit。


- 在Customer控制器中添加edit方法，在view中增加Edit.cshtml。

**Edit.cshtml**

```
@{
    ViewBag.Title = "Edit";
}

@model WebApiMVC.ViewModel.CustomerViewModel

<h2> Edit </h2>
<div align="center" width="500px">
    @using (Html.BeginForm("Edit", "Customer", FormMethod.Post))
    {
        <table class="table"
               width="400px"
               cellpadding="20">
            <tr class="btn-default">
                <td>
                    @Html.LabelFor(model => model.customer.Name)
                </td>

                <td>
                    @Html.TextBoxFor(model => model.customer.Name)
                </td>
            </tr>
            <tr>
                <td>
                    @Html.LabelFor(model => model.customer.Address)
                </td>

                <td>
                    @Html.TextBoxFor(model => model.customer.Address)
                </td>
            </tr>
            <tr>
                <td>
                    @Html.LabelFor(model => model.customer.MobileNo)
                </td>

                <td>
                    @Html.TextBoxFor(model => model.customer.MobileNo)
                </td>
            </tr>
            <tr>
                <td>
                    @Html.LabelFor(model => model.customer.Birthdate)
                </td>

                <td>
                    @Html.TextBoxFor(model => model.customer.Birthdate)
                </td>
            </tr>
            <tr>
                <td>
                    @Html.LabelFor(model => model.customer.EmailId)
                </td>

                <td>
                    @Html.TextBoxFor(model => model.customer.EmailId)
                </td>
            </tr>
            <tr>
                <td> </td>

                <td>
                    <input type="submit" value="保存" /> &nbsp; <a class="btn-primary" href="@Url.Action("Index ","Customer ")"> 返回 </a>
                    @Html.HiddenFor(model => model.customer.CustomerId)
                </td>
            </tr>
        </table>
    }
</div>

```
最终项目代码已经完成了。这时候我们运行项目试试看了。但还有一点要注意的是，我们必须同时运行web api和mvc项目。怎么办呢？其实只要在项目属性里设置就可以了。



- 右键解决方案，选择属性(Properties)。

![](https://i.imgur.com/HVlREAj.jpg)



- 接来下在启动项目选项中，选择`多项目启动`,在`Action`中设置两个项目同时启动。



- 运行项目，看看CRUD操作的执行结果。

Select
![](https://i.imgur.com/GkKF4HQ.jpg)

Insert
![](https://i.imgur.com/PeTHReA.jpg)


Update
![](https://i.imgur.com/a1PcMaO.jpg)


Delete
![](https://i.imgur.com/ieeFl3w.jpg)

这是整个技术预研的过程，本文末尾位置提供代码下载。


<div style="background: #f0f8ff; padding: 10px; border: 2px dashed #990d0d; font-family: 微软雅黑;">
&nbsp;作者：<a href="http://www.cnblogs.com/yayazi/">阿子</a>
<br>
&nbsp;博客地址：<a href="http://www.cnblogs.com/yayazi/">http://www.cnblogs.com/yayazi/</a>
<br>
&nbsp;本文地址：<a href="http://www.cnblogs.com/yayazi/p/8444054.html">http://www.cnblogs.com/yayazi/p/8444054.html</a>
<br>
&nbsp;声明：本博客原创文字允许转载，转载时必须保留此段声明，且在文章页面明显位置给出原文链接。
<br>
&nbsp;源码下载：<a href="https://github.com/daivven/WebApiMVC">https://github.com/daivven/WebApiMVC</a>

</div>


**PS：本人近期打算换工作，目标地点武汉，目标职位.Net软件工程师，期望月薪资12k以上，春节后可参加面试。有没有园友有合适岗位推荐,求内推哇~**
