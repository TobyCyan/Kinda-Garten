using System;

interface IHoldInteractable
{
    public event Action OnHoldCompleted;
    void DoOnHold();
    void DoWhileHold();
    void DoOnRelease();
}
