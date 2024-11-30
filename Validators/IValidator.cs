namespace UserInfoApp.Validators
{
    public interface IValidator<T>
    {
        (bool IsValid, string Error) Validate(T value);
        string Format(T value);
    }
}
