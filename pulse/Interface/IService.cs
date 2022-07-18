namespace pulse.Interface
{
    public interface IService
    {
        /// <summary>
        /// Вывод на экран меню сервиса
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PrintMenu(CancellationToken cancellationToken = default);
    }
}
