using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
/// <summary>
/// A command whose sole purpose is to 
/// relay its functionality to other
/// objects by invoking delegates. The
/// default return value for the CanExecute
/// method is 'true'.
/// </summary>
public class RelayCommand : ICommand
{
    #region Fields

    readonly Action<object> _execute;
    readonly Predicate<object> _canExecute;

    #endregion // Fields

    #region Constructors

    /// <summary>
    /// Creates a new command that can always execute.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    public RelayCommand(Action<object> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        if (execute == null)
            throw new ArgumentNullException("execute");

        _execute = execute;
        _canExecute = canExecute;
    }

    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameters)
    {
        return _canExecute == null ? true : _canExecute(parameters);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameters)
    {
        _execute(parameters);
    }

    #endregion // ICommand Members
}

public class RelayCommand<T> : ICommand
{
    private Predicate<T> _canExecute;
    private Action<T> _execute;

    public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    private void Execute(T parameter)
    {
        _execute(parameter);
    }

    private bool CanExecute(T parameter)
    {
        return _canExecute == null ? true : _canExecute(parameter);
    }

    public bool CanExecute(object parameter)
    {
        return parameter == null ? false : CanExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
        var temp = Volatile.Read(ref CanExecuteChanged);

        if (temp != null)
            temp(this, new EventArgs());
    }
}