using System.ComponentModel.DataAnnotations;

namespace Enrollment.Dtos.Requests;

public class ProfessorRegisterRequest
{
    [Required(ErrorMessage = "로그인 ID는 필수입니다.")]
    public string LoginId { get; set; }

    [Required(ErrorMessage = "비밀번호는 필수입니다.")]
    public string Pw { get; set; }

    [Required(ErrorMessage = "이름은 필수입니다.")]
    public string Name { get; set; }
}