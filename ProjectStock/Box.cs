using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStock
{
    /// <summary>
    /// Коробка склада 
    /// </summary>
    internal class Box
    {
        /// <summary>
        /// Срок годности продукта в днях от даты производства.
        /// </summary>
        private readonly int _day_keeping = 100;

        private DateTime _date_production;
        /// <summary>
        /// Дата производства 
        /// </summary>
        public DateTime DateProduction { get { return _date_production; } }

        private DateTime _date_expiration;
        /// <summary>
        /// Дата, когда заканчивается срок годности.
        /// </summary>
        public DateTime DateExpiration { get { return _date_expiration; } }

        private float _width;
        /// <summary>
        /// Ширина коробки в сантиметрах.
        /// </summary>
        public float Width { get { return _width; } }
        /// <summary>
        /// Высота коробки в сантиметрах.
        /// </summary>
        private float _height;
        public float Height { get { return _height; } }
        /// <summary>
        /// Глубина коробки в сантиметрах.
        /// </summary>
        private float _depth;
        public float Depth { get { return _depth; } }
        /// <summary>
        /// Масса коробки в килограммах.
        /// </summary>
        private float _weight;
        public float Weight { get { return _weight; } }

        /// <summary>
        /// Новый экземпляр коробки. Необходимо передать минимум один параметр даты: или дату производства, или дату окончания срока годности.
        /// </summary>
        /// <param name="width">Ширина в сантиметрах.</param>
        /// <param name="height">Высота в сантиметрах.</param>
        /// <param name="depth">Глубина в сантиметрах.</param>
        /// <param name="weight">Вес в килограммах.</param>
        /// <param name="date_production">Дата производства.</param>
        /// <param name="date_expiration">Дата, когда заканчивается срок годности.</param>
        /// <exception cref="ArgumentException">Необходимо передать минимум один параметр даты: или дату производства, или дату окончания срока годности.</exception>
        public Box(float width, float height, float depth, float weight, DateTime? date_production = null, DateTime? date_expiration = null) {
            if (date_production != null && date_expiration == null)
            {
                _date_production = (DateTime)date_production;
                _date_expiration = ((DateTime)date_production).AddDays(_day_keeping);
            }
            else if (date_expiration != null && date_production == null)
            {
                _date_production = ((DateTime)date_expiration).AddDays(-_day_keeping);
                _date_expiration = (DateTime)date_expiration;
            }
            else if (date_expiration != null && date_production != null)
            {
                _date_production = (DateTime)date_expiration;
                _date_expiration = (DateTime)date_expiration;
            }
            else { throw new ArgumentException("Пожалуйста, укажите дату производства или дату окончания срока годности"); }   

            _width = width;
            _height = height;
            _depth = depth;
            _weight = weight;
        }

        /// <summary>
        /// Вывести объем коробки.
        /// </summary>
        /// <returns>Объем коробки в кубических сантиметрах.</returns>
        public float GetVolume()
        {
            return _depth*_width*_height;
        }

    }
}
