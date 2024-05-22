//using Microsoft.Xna.Framework.Audio;

//namespace ZZZ.Framework.Auding.Components
//{
//    public class SoundRegistrar : BaseRegistrar<Component>
//    {
//        public ISoundListener CurrentListener => SoundListener.Instance;

//        private List<ISoundListener> listeners = new List<ISoundListener>();
//        private List<ISoundEmitter> emitters = new List<ISoundEmitter>();

//        public SoundRegistrar(GameManager gameManager) : base(gameManager)
//        {
//        }

//        protected override void Reception(Component component)
//        {
//            switch (component)
//            {
//                case ISoundEmitter emitter:

//                    emitters.Add(emitter); // Запоминаем все новые источники
//                    //emitter.Listener = CurrentListener; // устанапвливаем текущего слушателя

//                    break;

//                case ISoundListener soundListener:

//                    var lastListener = CurrentListener; // запоминаем старого слушателя

//                    listeners.Add( soundListener); // запоминаем всех новых слушателей

//                    if (lastListener == CurrentListener) // если слушатель поменялся
//                        break;

//                    foreach (var emitter in emitters)
//                    {
//                        //emitter.Listener = CurrentListener; // то меняем на нового слушателя
//                    }

//                    break;
//                default:
//                    break;
//            }
//        }
//        protected override void Departure(Component component)
//        {
//            switch (component)
//            {
//                case ISoundEmitter emitter:

//                    emitters.Remove(emitter); // забываем источник
//                    //emitter.Listener = null;

//                    break;

//                case ISoundListener soundListener:

//                    var lastListener = CurrentListener;

//                    listeners.Remove(soundListener); // забываем слушателя

//                    if (lastListener == CurrentListener) // если текущий слушатель поменялся 
//                        break;

//                    foreach (var emitter in emitters)
//                    {
//                        //emitter.Listener = CurrentListener; // то меняем у всех источников
//                    }
//                    break;
//                default:
//                    break;
//            }
//        }

//    }
//}
