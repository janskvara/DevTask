
namespace DevTask.Domain.Models
{
    public enum ETypeOfTransaction: byte
    {
        // increments balance
        Deposite = 0,

        // decrements balance
        Stake = 1,

        // increments balance
        Win = 2
    }
}
