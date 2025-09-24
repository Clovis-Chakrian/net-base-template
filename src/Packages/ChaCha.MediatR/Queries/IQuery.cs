namespace ChaCha.MediatR.Queries;

public interface IBaseQuery {}
public interface IQuery<out TResponse> : IBaseQuery { }
public interface IQuery : IBaseQuery { }