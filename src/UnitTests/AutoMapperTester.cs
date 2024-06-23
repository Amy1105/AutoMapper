using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AutoMapper.UnitTests.AutoMapperTester;

namespace AutoMapper.UnitTests;

public class AutoMapperTester : IDisposable
{
    [Fact]
    public void Should_be_able_to_handle_derived_proxy_types()
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ModelType, DtoType>());
        var source = new[] { new DerivedModelType { TheProperty = "Foo" }, new DerivedModelType { TheProperty = "Bar" } };

        var mapper = config.CreateMapper();
        var destination = (DtoType[])mapper.Map(source, typeof(ModelType[]), typeof(DtoType[]));

        destination[0].TheProperty.ShouldBe("Foo");
        destination[1].TheProperty.ShouldBe("Bar");
    }

    [Fact]
    public void t()
    {
        List<DepartmentDto> departments = new List<DepartmentDto>() {
             new DepartmentDto (){ Name="department11", Budget=11.1M},
             new DepartmentDto (){Name="department11", Budget=11.1M},
            };

        List<DepartmentDto> departments2 = new List<DepartmentDto>() {
             new DepartmentDto (){ Name="department22", Budget=22.1M},
             new DepartmentDto (){Name="department22", Budget=22.1M},
            };

        List<InstructorDto> instructorDtos = new List<InstructorDto>()
            {
              new InstructorDto(){
                  LastName="Kapoor111",FirstMidName="Candace111"
              ,Departments=departments
              },
              new InstructorDto(){LastName="Kapoor111",FirstMidName="Candace111"
              ,Departments=departments2
              }
            };

        CourseDto courseDto = new CourseDto()
        {
            Title = "Chemistry111",
            Instructors = instructorDtos
        };     

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<InstructorDto, Instructor>();
            cfg.CreateMap<CourseDto, Course>();                       
            cfg.CreateMap<DepartmentDto, Department>();
        });
        var mapper = config.CreateMapper();
        var destination = mapper.Map<Course>(courseDto);
    }

    [Fact]
    public void t2()
    {
        var orderDto = new ex_OrderDto
        {
            OrderNumber = "20210801",
            Details = new List<ex_OrderDetailDto>
                {
                    new ex_OrderDetailDto {  ItemName = "Item1" },
                    new ex_OrderDetailDto {  ItemName = "Item2" }
                }
        };
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ex_OrderDetailDto, ex_OrderDetail>();   //往 Profile 类中的List<TypeMapConfiguration> _typeMapConfigs 对象中添加
            cfg.CreateMap<ex_OrderDto, ex_Order>();           
        });
        var mapper = config.CreateMapper();     
        var order = mapper.Map<ex_Order>(orderDto);
    }

    #region
    public class ex_Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string OrderName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<ex_OrderDetail> Details { get; set; }
    }

    public class ex_OrderDetail
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int Count { get; set; }
    }


    public class ex_OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public List<ex_OrderDetailDto> Details { get; set; }
    }

    public class ex_OrderDetailDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
    }

    #endregion


    #region DTO
    public class CourseDto
    {
        public int CourseID { get; set; }
        public string Title { get; set; } = string.Empty;

       // public int? Credits { get; set; }

        public List<InstructorDto> Instructors { get; set; }
    }

    public class InstructorDto
    {
        public int ID { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }

        public List<DepartmentDto> Departments { get; set; }
    }

    public class DepartmentDto
    {
        public int DepartmentID { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

    }

    #endregion

    #region dbmodel
    /// <summary>
    /// 课程
    /// </summary>
    public class Course
    {      
        public int CourseID { get; set; }      
        public string Title { get; set; }

        //[Range(0, 10)]
        //public int? Credits { get; set; }

        //public bool IsDeleted { get; set; } = false;

        //public ICollection<Enrollment> Enrollments { get; set; }
        public List<Instructor> Instructors { get; set; }
    }

    public class Instructor
    {
        public int ID { get; set; }
   
        public string LastName { get; set; } 
       
        public string FirstMidName { get; set; }
     
        //public DateTime HireDate { get; set; }

     
        //public int CourseID { get; set; }

        //public Course Course { get; set; } = new Course();
      
        public List<Department> Departments { get; set; }

    }


    public class Department
    {
        public int DepartmentID { get; set; }

      
        public string Name { get; set; }

      
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                       ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }       
    }

    #endregion

    public void Dispose()
    {
        
    }

    public class ModelType
    {
        public string TheProperty { get; set; }
    }

    public class DerivedModelType : ModelType
    {
    }

    public class DtoType
    {
        public string TheProperty { get; set; }
    }
}