# 수강신청 서비스 백엔드 RestAPI서버

## 🎄 간단한 작동 모습

- 학생페이지
  ![학생소개](https://user-images.githubusercontent.com/72899681/230794427-3aa97161-3c95-4010-a737-250b71262869.gif)
- 교수페이지
  ![교수소개](https://user-images.githubusercontent.com/72899681/230793785-1a30177a-ff8d-42de-8089-807c9942729f.gif)

## 📮 요구사항

- 교수
    - 로그인 할 수 있다
    - 코스를 개설할 수 있다
        - 수강 금지 과 설정 가능
    - 학생에게 성적을 부여할 수 있다
        - 성적이 부과되면, 수강중인 과목이 수강완료 처리된다
        - 재수강의 경우, 기존 성적은 사라지고 새 점수로 대체된다
            - 재수강시, 과거 수강 내역은 DB에서 완전 삭제된다

- 학생
    - 로그인 할 수 있다
    - 수강신청을 할 수 있다
    - 장바구니 기능을 이용할 수 있다
    - 수강완료된 과목들을 볼 수 있다
    - B0미만일경우 재수강을 신청할 수 있다. 이때, 동일한 과목 코드를 가지고 있어야한다

- 어드민
    - 과를 개설할 수 있다
    - 수업교실을 만들 수 있다
    - 과목을 만들 수 있다

## 🎫 비즈니스 로직

- 각 enitity들의 code는 중복되게 만들 수 없다
- 수업장소와 수업시간이 겹치는 코스는 생성할 수 없다
- 수강정원을 넘겨서 수강신청 할 수 없다
- 학생은 기존에 신청한 코스와 시간이 겹치는 코스를 신청할 수 없다
- 이미 수강한 코스는 수강신청할 수 없다(단, B0미만일 경우 가능하다)
- 수강금지과 학생은 수강신청할 수 없다
- 이미 신청한 코스는 다시 신청할 수 없다

## 📃 기술스택

- Framework: .NET 9.0
- Web Framework: ASP.NET Core Web API
- Database: SQLite
- ORM: Entity Framework Core
- API Documentation: Swashbuckle (Swagger)
- Authentication: Cookie Authentication
- Password Hashing: BCrypt.Net-Next
- Object Mapping: AutoMapper

## 🔍 기술적 구현 사항
- Global Exception Handling: GlobalExceptionHandlingMiddleware를 구현하여 애플리케이션 전역에서 발생하는 예외를 일관된 JSON 형식(ApiExceptionDto)으로 처리합니다.
- Custom Model Validation: Program.cs에서 InvalidModelStateResponseFactory를 재정의하여, 모델 바인딩 및 유효성 검사 실패 시(예: JsonException 또는 빈 body 요청) 커스텀된 오류 응답을 반환합니다.
- Authentication: 쿠키 기반 인증을 사용하며, Program.cs에서 쿠키 이름을 "JSESSIONID"로 커스텀하고 30분의 만료 시간을 설정합니다.
- Dependency Injection: Program.cs에서 Scoped 생명주기로 IAdminService, ICourseService 등 각종 서비스와 IPasswordHasher, IAuthHelper 같은 유틸리티를 주입하여 사용합니다.
- Data Access (EF Core):
  - ApplicationDbContext를 통해 데이터베이스와 상호작용합니다.
  - OnModelCreating 메서드 내에서 OwnsOne을 사용하여 MemberInfo, CourseTime 같은 값 객체(Value Object)를 모델에 포함시킵니다.
  - HasIndex().IsUnique()를 사용하여 Department, Subject 등의 Code 속성에 Unique 제약 조건을 설정함으로써 "각 enitity들의 code는 중복되게 만들 수 없다"는 비즈니스 로직을 데이터베이스 수준에서 보장합니다.
- DTOs & AutoMapper:
  - 요청(Request)과 응답(Response)에 DTO (Data Transfer Object) 패턴을 적용하여 API의 인터페이스를 명확하게 정의합니다 (예: AdminRegisterRequest, CourseResponse).
  - MappingProfile.cs에서 AutoMapper를 사용하여 엔티티 모델(예: Student)과 DTO(예: LoginResponse) 간의 매핑을 설정합니다.


### [눌러서 프론트 소개 페이지로 가기](https://github.com/hyeongcheolkim/enrollmentFront)