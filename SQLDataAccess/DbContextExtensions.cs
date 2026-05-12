using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace SQLDataAccess
{
    public static class DbContextExtensions
    {
        public static void ApplyStateChanges(this DbContext dbContext)
        {
            foreach (var dbEntityEntry in dbContext.ChangeTracker.Entries())
            {
                var entityState = dbEntityEntry.Entity as BaseData;
                if (entityState == null)
                {
                    throw new InvalidCastException(
                    "All entites must implement " +
                    "the IObjectState interface, this interface " +
                    "must be implemented so each entites state" +
                    "can explicitely determined when updating graphs.");
                }

                dbEntityEntry.State = GetEntityState(entityState.State);
            }
        }

        private static EntityState GetEntityState(EDataState state)
        {
            switch (state)
            {
                case EDataState.Added:
                    return EntityState.Added;
                case EDataState.Modified:
                    return EntityState.Modified;
                case EDataState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

    }
}
