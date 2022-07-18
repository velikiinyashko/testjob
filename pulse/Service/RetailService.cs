namespace pulse.Service
{
    public class RetailService : IService
    {
        public string Header => "Торговые точки:";

        private readonly IRepository<Retail> _repository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<pulse.Model.Party> _partyRepository;

        Dictionary<ConsoleKey, GetEventRecord> _events;

        public RetailService()
        {
            #region Регистрируем репозитории
            _repository = new RetailRepository();
            _stockRepository = new StockRepository();
            _partyRepository = new PartyRepository();
            #endregion

            #region Заполняем меню
            _events = new()
            {
                {ConsoleKey.D1, new GetEventRecord("1) Добавить торговую точку", this.AddRetail) },
                {ConsoleKey.D2, new GetEventRecord("2) Добавить склад", this.AddStock) },
                {ConsoleKey.D3, new GetEventRecord("3) Добавить партию", this.AddParty) },
                {ConsoleKey.D4, new GetEventRecord("4) Удалить тогровую точку", this.DeleteRetail) },
                {ConsoleKey.D5, new GetEventRecord("5) Удалить склад", this.DeleteStock) },
                {ConsoleKey.D6, new GetEventRecord("6) Удалить партию", this.DeleteParty) },

            };
            #endregion
        }

        public async Task PrintMenu(CancellationToken cancellationToken = default)
        {
            while (true)
            {

                Console.Clear();
                Header.PrintLineColor(ConsoleColor.Green);
                await GetAllRetail(cancellationToken);

                _events.PrintMenu();

                Console.Write("Выберете пункт меню:");

                var _inputKey = Console.ReadKey();

                if (_inputKey.Key == ConsoleKey.Escape)
                    return;

                if (_events.ContainsKey(_inputKey.Key))
                    await _events[_inputKey.Key].Event(cancellationToken);

            }
        }

        /// <summary>
        /// Вывод списка аптек
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        private async Task GetAllRetail(CancellationToken cancellation = default)
        {
            var retails = await _repository.GetAllAsync(cancellation);
            foreach (var item in retails)
                $"[{item.RetailId}] {item.Name} Адрес: {item.City}, {item.Address}; Телефон: {item.Phone};\r\nКол-во складов: {item.CountStocks}; Кол-во партий: {item.CountParty}; Товара на сумму: {item.SumPrice} \r\n".PrintLineColor(ConsoleColor.White);

        }

        /// <summary>
        /// Вывод списка аптек
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        private async Task GetStock(int RetailId, CancellationToken cancellation = default)
        {
            List<Stock> result = new();
            if (RetailId != 0)
                result = await _stockRepository.GetAsync(RetailId, cancellation);
            else
                result = await _stockRepository.GetAllAsync(cancellation);
            foreach (var item in result)
                $"[{item.StockId}] Название склада: {item.Name}".PrintLineColor(ConsoleColor.White);
        }

        #region AddRetail
        /// <summary>
        /// Метод добавления тороговой точки
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task AddRetail(CancellationToken cancellationToken = default)
        {
            Retail retail = new();
            Console.Clear();
            "Укажите данные торговой точки:".PrintLineColor(ConsoleColor.Green);
            ColorPrint.Separator();
            Console.Write("Название: ");
            retail.Name = Console.ReadLine();
            ColorPrint.Separator();
            Console.Write("Город: ");
            retail.City = Console.ReadLine();
            ColorPrint.Separator();
            Console.Write("Адрес: ");
            retail.Address = Console.ReadLine();
            ColorPrint.Separator();
            Console.Write("Телефон: ");
            retail.Phone = Console.ReadLine();
            ColorPrint.Separator();
            var res = await _repository.Create(retail, cancellationToken);
            if (res)
            {
                "Торговая точка сохранена. Нажмите любую клавишу для продолжения...".PrintLineColor(ConsoleColor.Green);
                Console.ReadKey();
            }
        }
        #endregion

        /// <summary>
        /// Метод удаления торговой точки
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task DeleteRetail(CancellationToken cancellationToken = default)
        {
            Console.Clear();
            await GetAllRetail(cancellationToken);
            Console.Write("\r\nИд удаляемой точки: ");

            var _retailId = Console.ReadLine().ToInt();
            while (true)
            {
                Console.Clear();
                await GetStock(_retailId, cancellationToken);
                "Вы действительно хотите удалить эту точку?\r\nЭто также удалит все партии для этого склада! [n/y]".PrintLineColor(ConsoleColor.Red);
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.N)
                    return;

                if (input.Key != ConsoleKey.Y)
                    continue;

                await _repository.Delete(_retailId, cancellationToken);
            }
        }

        #region AddStock
        /// <summary>
        /// Метод добавления склада
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task AddStock(CancellationToken cancellationToken = default)
        {
            Stock stock = new();
            Console.Clear();
            "Укажите данные склада:".PrintLineColor(ConsoleColor.Green);
            ColorPrint.Separator();
            Console.Write("Название: ");
            stock.Name = Console.ReadLine();
            ColorPrint.Separator();
            await GetAllRetail();
            Console.Write("Ид торговой точки: ");
            stock.RetailId = Console.ReadLine().ToInt();
            ColorPrint.Separator();

            var res = await _stockRepository.Create(stock, cancellationToken);
            if (res)
            {
                "Склад сохранен. Нажмите любую клавишу для продолжения...".PrintLineColor(ConsoleColor.Green);
                Console.ReadKey();
            }
        }
        #endregion

        /// <summary>
        /// Метод удаления склада
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task DeleteStock(CancellationToken cancellationToken = default)
        {
            Console.Clear();
            await GetStock(0, cancellationToken);
            Console.Write("\r\nИд удаляемого склада: ");

            var _stockId = Console.ReadLine().ToInt();
            while (true)
            {
                Console.Clear();
                await GetStock(_stockId, cancellationToken);
                "Вы действительно хотите удалить этот склад?\r\nЭто также удалит все партии для этого склада! [n/y]".PrintLineColor(ConsoleColor.Red);
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.N)
                    return;

                if (input.Key != ConsoleKey.Y)
                    continue;

                await _stockRepository.Delete(_stockId, cancellationToken);
                return;
            }

        }

        /// <summary>
        /// Метод добавления партии 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task AddParty(CancellationToken cancellationToken = default)
        {
            Party party = new();
            Console.Clear();
            "Создание партии: ".PrintLineColor(ConsoleColor.Green);
            ColorPrint.Separator();
            await GetAllRetail();
            Console.Write("Ид торговой точки: ");
            party.RetailId = Console.ReadLine().ToInt();

            ColorPrint.Separator();
            await GetStock(party.RetailId);
            Console.Write("Ид склада: ");
            party.StockId = Console.ReadLine().ToInt();

            //ColorPrint.Separator();
            //await GetProduct();
            //Console.Write("Ид склада: ");
            //party.StockId = Console.ReadLine().ToInt();

            ColorPrint.Separator();
            Console.Write("Количество: ");
            party.Count = Console.ReadLine().ToInt();

            ColorPrint.Separator();
            Console.Write("Стоимость: ");
            party.Price = Console.ReadLine().ToDecimal();

            var res = await _partyRepository.Create(party, cancellationToken);
            if (res)
            {
                "Партия сосоздана. Нажмите любую клавишу для продолжения...".PrintLineColor(ConsoleColor.Green);
                Console.ReadKey();
            }

        }

        /// <summary>
        /// Метод удаления партий
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task DeleteParty(CancellationToken cancellationToken = default)
        {
            Console.Clear();
            await GetAllRetail(cancellationToken);
            Console.Write("\r\nИд удаляемой партии: ");

            var _retailId = Console.ReadLine().ToInt();
            while (true)
            {
                Console.Clear();
                await GetStock(_retailId, cancellationToken);
                "Вы действительно хотите удалить эту партию?\r\nЭто также удалит все партии для этого склада! [n/y]".PrintLineColor(ConsoleColor.Red);
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.N)
                    return;

                if (input.Key != ConsoleKey.Y)
                    continue;

                await _repository.Delete(_retailId, cancellationToken);
            }
        }
    }
}
