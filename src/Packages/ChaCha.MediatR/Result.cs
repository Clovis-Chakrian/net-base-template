using FluentValidation.Results;

namespace ChaCha.MediatR;

public abstract class BaseResult
{
  public bool IsValid { get; protected set; }
  public ValidationResult ValidationResult { get; protected set; }

  protected BaseResult()
  {
    IsValid = true;
    ValidationResult = new ValidationResult();
  }

  public virtual void AddFailure(string error)
  {
    IsValid = false;
    ValidationResult.Errors.Add(new ValidationFailure(string.Empty, error));
  }

  public virtual void AddFailure(string property, string error)
  {
    IsValid = false;
    ValidationResult.Errors.Add(new ValidationFailure(property, error));
  }

  public virtual void HandleResult(ValidationResult validationResult)
  {
    IsValid = validationResult.IsValid;
  }
}

public class Result<T> : BaseResult
{
  public T? Data { get; private set; }
  private Result() : base()
  {
    Data = default;
  }

  public static Result<T> Create()
  {
    return new Result<T>();
  }

  public Result<T> Success(T data)
  {
    Data = data;
    this.IsValid = true;
    return this;
  }
}

public class Result : BaseResult
{
  private Result() : base()
  {
  }

  public static Result Create()
  {
    return new Result();
  }

  public Result Success()
  {
    this.IsValid = true;
    return this;
  }
}