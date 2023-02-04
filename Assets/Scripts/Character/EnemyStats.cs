using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField]
    private CharacterAnimator animator;

    private void OnCollisionEnter(Collision other)
    {
        Dev.Log("Enemy OnCollisionEnter: " + other.gameObject.name);
        if (other.gameObject.TryGetComponent(out IDamageDealer dealer))
        {
            Dev.Log("Enemy Receive Damage");
            // elements attacking
            this.ReceiveDamage(dealer);
            //this.ShowDamage(dealer);
        }
        else
        {
            Dev.Log("Enemy Not Receiving Damage");
        }
    }

    protected override void ShowDamage(IDamageDealer dealer)
    {
        var damageVal = (int)dealer.GetDamageValue();
        Dev.Log("Enemy Show Damage");
        if (damageVal > 0)
        {
            Color color = Core.Ins.UIEffectsManager.GetColorForElement(dealer.GetDamageElement());
            Core.Ins.UIEffectsManager.RequestPopUp(this.transform, damageVal.ToString(), color);
        }
    }

    protected override void Die()
    {
        Dev.Log("Enemy Die");
        base.Die();
        animator.PlayDeath();

        Destroy(gameObject, 4f);
    }
}