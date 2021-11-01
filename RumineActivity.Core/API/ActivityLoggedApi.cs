using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    //Логгированное АПИ
    public class ActivityLoggedApi : IActivityApi
    {
        public override string ToString()
        {
            return $"Логгированный {Main}";
        }

        protected readonly IActivityApi Main;

        public ActivityLoggedApi(IActivityApi main)
        {
            Main = main;
        }

        public async Task<Post> Create(Post post)
        {
            Console.WriteLine($"{Main}: Создание поста {post}");
            var result = await Main.Create(post);
            if (result != null)
                Console.WriteLine($"{Main}: Пост успешно создан");
            else
                Console.WriteLine($"{Main}: Не удалось создать пост");
            return result;
        }
        public async Task<Topic> Create(Topic topic)
        {
            Console.WriteLine($"{Main}: Создание темы {topic}");
            var result = await Main.Create(topic);
            if (result != null)
                Console.WriteLine($"{Main}: Тема успешно создана");
            else
                Console.WriteLine($"{Main}: Не удалось создать тему");
            return result;
        }


        public async Task<Post> Delete(Post post)
        {
            Console.WriteLine($"{Main}: Удаление поста {post}");
            var result = await Main.Delete(post);
            if (result != null)
                Console.WriteLine($"{Main}: Пост успешно удален");
            else
                Console.WriteLine($"{Main}: Не удалось удалить пост");
            return result;
        }
        public async Task<Topic> Delete(Topic topic)
        {
            Console.WriteLine($"{Main}: Удаление темы {topic}");
            var result = await Main.Delete(topic);
            if (result != null)
                Console.WriteLine($"{Main}: Тема успешно удалена");
            else
                Console.WriteLine($"{Main}: Не удалось удалить тему");
            return result;
        }


        public async Task<IForum> GetForum()
        {
            Console.WriteLine($"{Main}: Получение форума");
            var result = await Main.GetForum();
            if (result != null)
                Console.WriteLine($"{Main}: Форум успешно получен");
            else
                Console.WriteLine($"{Main}: Не удалось получить форум");
            return result;
        }
        public async Task<IForum> GetForum(DateRange range)
        {
            return await GetForum();
        }


        public async Task<Post> Update(Post post)
        {
            Console.WriteLine($"{Main}: Обновление поста {post}");
            var result = await Main.Update(post);
            if (result != null)
                Console.WriteLine($"{Main}: Пост успешно обновлен");
            else
                Console.WriteLine($"{Main}: Не удалось обновить пост");
            return result;
        }
        public async Task<Topic> Update(Topic topic)
        {
            Console.WriteLine($"{Main}: Обновление темы {topic}");
            var result = await Main.Update(topic);
            if (result != null)
                Console.WriteLine($"{Main}: Тема успешно обновлена");
            else
                Console.WriteLine($"{Main}: Не удалось обновить тему");
            return result;
        }
    }
}
