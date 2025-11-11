using AutoMapper;
using Enrollment.Dtos.Requests;
using Enrollment.Dtos.Response;
using Enrollment.Models;

namespace Enrollment.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Admin, LoginResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MemberInfo.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "LOGIN_ADMIN")) 
            .ForMember(dest => dest.DepartmentName, opt => opt.Ignore()); 

        CreateMap<AdminRegisterRequest, Admin>()
            .ForMember(dest => dest.MemberInfo, 
                       opt => opt.MapFrom(src => new MemberInfo 
                       { 
                           LoginId = src.LoginId, 
                           Name = src.Name, 
                           Activated = true 
                       }));
        
        CreateMap<Professor, LoginResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MemberInfo.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "LOGIN_PROFESSOR")) 
            .ForMember(dest => dest.DepartmentName, opt => opt.Ignore()); 

        CreateMap<ProfessorRegisterRequest, Professor>()
            .ForMember(dest => dest.MemberInfo,
                       opt => opt.MapFrom(src => new MemberInfo
                       {
                           LoginId = src.LoginId,
                           Name = src.Name,
                           Activated = true
                       }));


        CreateMap<Student, LoginResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MemberInfo.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "LOGIN_STUDENT")) 
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name)); 

        CreateMap<StudentRegisterRequest, Student>()
            .ForMember(dest => dest.MemberInfo,
                       opt => opt.MapFrom(src => new MemberInfo
                       {
                           LoginId = src.LoginId,
                           Name = src.Name,
                           Activated = true
                       }))
            .ForMember(dest => dest.Department, opt => opt.Ignore()); 


        CreateMap<Basket, BasketResponse>()
            .ForMember(dest => dest.BasketId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.MemberInfo.Name))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Course.Capacity))
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Course.Subject.Id))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Course.Subject.Name))
            .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Course.Division))
            .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Course.Subject.Code));


        CreateMap<CreateDepartmentRequest, Department>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Activated, opt => opt.MapFrom(src => true)); 

        CreateMap<Department, DepartmentResponse>()
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Code));
        

        CreateMap<Classroom, ClassroomResponse>()
            .ForMember(dest => dest.ClassroomId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ClassroomName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ClassroomCode, opt => opt.MapFrom(src => src.Code));


        CreateMap<SubjectMakeRequest, Subject>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Credit, opt => opt.MapFrom(src => src.Credit))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Activated, opt => opt.MapFrom(src => true)); 

        CreateMap<Subject, SubjectResponse>()
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Credit, opt => opt.MapFrom(src => src.Credit))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));


        CreateMap<Course, CourseResponse>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Subject.Id))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Subject.Code))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.Id))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.StudentYear, opt => opt.MapFrom(src => src.StudentYear))
            .ForMember(dest => dest.OpenYear, opt => opt.MapFrom(src => src.OpenYear))
            .ForMember(dest => dest.OpenSemester, opt => opt.MapFrom(src => src.OpenSemester))
            .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division))
            .ForMember(dest => dest.ProfessorId, opt => opt.MapFrom(src => src.Professor.Id))
            .ForMember(dest => dest.ProfessorName, opt => opt.MapFrom(src => src.Professor.MemberInfo.Name)) 
            .ForMember(dest => dest.CourseTime, opt => opt.MapFrom(src => src.CourseTime))
            .ForMember(dest => dest.ProhibitedDepartments, 
                       opt => opt.MapFrom(src => src.ProhibitedDepartments)); 
                       

        CreateMap<Enroll, OnSemesterEnrollResponse>()
            .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Id)) 
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Course.Subject.Id))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Course.Subject.Name))
            .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Course.Subject.Code))
            .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Course.Division))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.MemberInfo.Name))
            .ForMember(dest => dest.StudentDepartmentName, opt => opt.MapFrom(src => src.Student.Department.Name));


        CreateMap<Enroll, NotSemesterEnrollResponse>()
            .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Course.Subject.Id))
            .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Course.Subject.Code))
            .ForMember(dest => dest.SubjectCredit, opt => opt.MapFrom(src => src.Course.Subject.Credit))
            .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Course.Division))
            .ForMember(dest => dest.CourseOpenYear, opt => opt.MapFrom(src => src.Course.OpenYear))
            .ForMember(dest => dest.CourseOpenSemester, opt => opt.MapFrom(src => src.Course.OpenSemester))
            .ForMember(dest => dest.ScoreType, opt => opt.MapFrom(src => src.ScoreType))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Course.Subject.Name));
    }
}