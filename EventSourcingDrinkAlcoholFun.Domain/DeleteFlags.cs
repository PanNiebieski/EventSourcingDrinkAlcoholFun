using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Domain
{
    public static class DeleteFlags
    {
        private static List<DeleteIngredient> _delete = new List<DeleteIngredient>();

        public static void AddDeleteFlag(int drinkid, int ingredientid)
        {
            _delete.Add(new(drinkid, ingredientid));
        }

        public static List<DeleteIngredient> Get()
        {
            return _delete;
        }

        public static void Clear()
        {
            _delete.Clear();
        }
    }

    public record DeleteIngredient(int drinkid,int ingredientid);
}
