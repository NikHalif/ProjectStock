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
    public class Box
    {
        private ulong _id;
        public ulong ID { get { return _id; }
            set
            {
                if (value != _id)
                    _id = value;
            } }

        /// <summary>
        /// Срок годности продукта в днях от даты производства.
        /// </summary>
        private readonly int _day_keeping = 100;

        private DateTime _date_production;
        /// <summary>
        /// Дата производства 
        /// </summary>
        public DateTime DateProduction { 
            get { return _date_production; } 
            set { if (value != _date_production) _date_production = value; } }

        private DateTime _date_expiration;
        /// <summary>
        /// Дата, когда заканчивается срок годности.
        /// </summary>
        public DateTime DateExpiration { 
            get { return _date_expiration; } 
            set { if (value != _date_expiration) _date_expiration = value; } }

        private float _width;
        /// <summary>
        /// Ширина коробки в сантиметрах.
        /// </summary>
        public float Width { get { return _width; }
            set
            {
                if (value != _width)
                    _width = value;
            } }
        private float _height;
        /// <summary>
        /// Высота коробки в сантиметрах.
        /// </summary>
        public float Height { get { return _height; }
            set
            {
                if (value != _height)
                    _height = value;
            } }
        private float _depth;
        /// <summary>
        /// Глубина коробки в сантиметрах.
        /// </summary>
        public float Depth { get { return _depth; }
            set
            {
                if (value != _depth)
                    _depth = value;
            } }
        private float _mass;
        /// <summary>
        /// Масса коробки в килограммах.
        /// </summary>
        public float Mass { get { return _mass; }
            set
            {
                if (value != _mass)
                    _mass = value;
            } }

        public Box()
        {

        }

        /// <summary>
        /// Новый экземпляр коробки. Необходимо передать минимум один параметр даты: или дату производства, или дату окончания срока годности.
        /// </summary>
        /// <param name="id">Номер коробки в системе.</param>
        /// <param name="width">Ширина в сантиметрах.</param>
        /// <param name="height">Высота в сантиметрах.</param>
        /// <param name="depth">Глубина в сантиметрах.</param>
        /// <param name="mass">Вес в килограммах.</param>
        /// <param name="date_production">Дата производства.</param>
        /// <param name="date_expiration">Дата, когда заканчивается срок годности.</param>
        /// <exception cref="ArgumentException">Необходимо передать минимум один параметр даты: или дату производства, или дату окончания срока годности.</exception>
        public Box(ulong id, float width, float height, float depth, float mass, DateTime? date_production = null, DateTime? date_expiration = null) {
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
            else if(date_production >= date_expiration) throw new ArgumentException("Некорректная дата производства или срока годности! Дата производства должна быть меньше даты окончания срока годности.");
            else throw new ArgumentException("Некорректные параметры дат. Укажите дату производства или дату окончания срока годности."); 

            _id = id;
            _width = width;
            _height = height;
            _depth = depth;
            _mass = mass;
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
