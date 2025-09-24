namespace ChaCha.MediatR.Commands;

public interface IBaseCommand {}
public interface ICommand<out TResponse> : IBaseCommand { }
public interface ICommand : IBaseCommand { }