using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStock
{
    /// <summary>
    /// Паллета склада.
    /// </summary>
    internal class Pallet
    {
        private ulong _id;
        public ulong ID { get { return _id; } }
        /// <summary>
        /// Высота поддона паллеты.
        /// </summary>
        private float height_pallet = 14.5f;

        /// <summary>
        /// Масса поддона паллеты.
        /// </summary>
        private float mass_pallet = 30;

        private List<Box> _boxes;
        /// <summary>
        /// Список коробок на паллете.
        /// </summary>
        public List<Box> Boxes { get { return _boxes; } }

        private float _width;
        /// <summary>
        /// Ширина паллеты в сантиметрах.
        /// </summary>
        public float Width { get { return _width; } }

        private float _depth;
        /// <summary>
        /// Длина паллеты в сантиметрах.
        /// </summary>
        public float Depth { get { return _depth; } }

        /// <summary>
        /// Экземпляр паллеты.
        /// </summary>
        /// <param name="id">Номер паллеты в системе.</param>
        /// <param name="width">Ширина в сантиметрах.</param>
        /// <param name="depth">Длина в сантиметрах.</param>
        public Pallet(ulong id, float width, float depth) {
            _id = id;
            _depth = depth;
            _width = width;
            _boxes = new List<Box>();
        }

        /// <summary>
        /// Экземпляр паллеты.
        /// </summary>
        /// <param name="id">Номер паллеты в системе.</param>
        /// <param name="width">Ширина в сантиметрах.</param>
        /// <param name="depth">Длина в сантиметрах.</param>
        /// <param name="boxes">Список коробок на паллете.</param>
        public Pallet(ulong id, float width, float depth, List<Box> boxes)
        {
            _id = id;
            _depth = depth;
            _width = width;
            _boxes = boxes;
            
        }

        /// <summary>
        /// Проверка допустимого размера коробки для данной паллеты.
        /// </summary>
        /// <param name="box">Экземпляр коробки.</param>
        /// <returns>Возвращает true, если ширина или глубина коробки больше ширины или длины паллеты. В ином случае false.</returns>
        protected bool IsWidthOrDepthValueBoxMax(Box box) => (box.Depth > _depth || box.Width > _width) && (box.Depth > _width || box.Width > _depth);

        /// <summary>
        /// Добавление коробки в коллекцию. Ширина и глубина коробки не могут быть больше ширины и глубины паллеты.
        /// </summary>
        /// <param name="box">Экземпляр коробки.</param>
        /// <exception cref="ArgumentException">Ширина и глубина коробки не могут быть больше ширины и глубины паллеты.</exception>
        public void Add(Box box)
        {
            if(!IsWidthOrDepthValueBoxMax(box)) _boxes.Add(box);
            else throw new ArgumentException("Ширина и глубина коробки не могут быть больше ширины и глубины паллеты.");
        }

        /// <summary>
        /// Удаление экземпляра коробки из списка. 
        /// </summary>
        /// <param name="box">Экземпляр коробки.</param>
        public void Remove(Box box) => _boxes.Remove(box);
        /// <summary>
        /// Получение объем паллеты с коробками.
        /// </summary>
        /// <returns>Объем поддона паллеты и всех коробок на паллете в кубических сантиметрах.</returns>
        public float GetVolumePallet() => _boxes.Sum(t => t.GetVolume()) + (height_pallet * _depth * _width);

        /// <summary>
        /// Получение масса паллеты и коробок на паллете.
        /// </summary>
        /// <returns>Масса в килограммах.</returns>
        public float GetMass() => _boxes.Sum(t => t.Mass) + mass_pallet;

        /// <summary>
        /// Поиск первой коробки, подходящей условиям предиката. 
        /// </summary>
        /// <param name="predicate">Предикат.</param>
        /// <returns>Экземпляр коробки, подходящий под условия. Null, если нет совпадений.</returns>
        public Box? Find(Predicate<Box> predicate) => _boxes.Find(predicate);
        /// <summary>
        /// Поиск всех коробок, подходящих условиям предиката. 
        /// </summary>
        /// <param name="predicate">Предикат.</param>
        /// <returns>Список коробок, подходящих условиям предиката. Список пуст, если нет совпадений.</returns>
        public List<Box> FindAll(Predicate<Box> predicate) => _boxes.FindAll(predicate);

        /// <summary>
        /// Получение даты окончания срока годности для паллеты. 
        /// </summary>
        /// <returns>Минимальная дата окончания срока годности из коробок на паллете.</returns>
        public DateTime? GetDateExpiration()
        {
            try
            {
                return _boxes.Min(t => t.DateExpiration);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

    }

}
