namespace Svelto.ECS.Example.Survive
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<PickupInfo>
    {
        public void Ready()
        { }

        public HealthEngine(ISequencer damageSequence, ISequencer pickupSequence)
        {
            _damageSequence = damageSequence;
            _pickupSequence = pickupSequence;
        }

        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Step(ref PickupInfo pickup, int type)
        {
            var entityView = entityViewsDB.QueryEntityView<HealthEntityView>(pickup.triggerEntityID);
            var healthComponent = entityView.healthComponent;

            healthComponent.currentHealth += pickup.amount;
            if (healthComponent.currentHealth > 100)
                healthComponent.currentHealth = 100;

            _pickupSequence.Next(this, ref pickup, type);
        }

        public void Step(ref DamageInfo damage, int condition)
        {
            var entityView      = entityViewsDB.QueryEntityView<HealthEntityView>(damage.entityDamagedID);
            var healthComponent = entityView.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            //the HealthEngine can branch the sequencer flow triggering two different
            //conditions
            if (healthComponent.currentHealth <= 0)
                _damageSequence.Next(this, ref damage, DamageCondition.Dead);
            else
                _damageSequence.Next(this, ref damage, DamageCondition.Damage);
        }

        readonly ISequencer  _damageSequence;
        readonly ISequencer _pickupSequence;
    }
}
