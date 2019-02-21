namespace DataAcquisitions.DataCollector
{
    class Factory
    {

        public object CreateComponent()
        {
            DataCollectorParameters parameters = new DataCollectorParameters();

            return new DataCollector(parameters);
        }

    }
}
