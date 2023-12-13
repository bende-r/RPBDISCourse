namespace VideoRentalWeb.Models
{
    public enum SortState
    {
        No,

        //Genres
        GenreTitleAsc,
        GenreTitleDesc,

        //Disks
        DiskTitleAsc,
        DiskCreationYearAsc,
        DiskProducerAsc,
        MainActorAsc,
        RecordingDateAsc,
        GenreIdAsc,
        TypeIdAsc,

        DiskTitleDesc,
        DiskCreationYearDesc,
        DiskProducerDesc,
        MainActorDesc,
        RecordingDateDesc,
        GenreIdDesc,
        TypeIdDesc,

        //Clientele
        ClientSurnameAsc,
        ClientNameAsc,
        ClientMiddlenameAsc,
        ClientAddressAsc,
        ClientPhoneAsc,
        ClientPassportAsc,

        ClientPhoneDesc,
        ClientPassportDesc,
        ClientAddressDesc,
        ClientMiddlenameDesc,
        ClientNameDesc,
        ClientSurnameDesc,

        //Types
        TypeTitleAsc,
        TypeTitleDesc,

        //Positions
        PositionTitleAsc,
        PositionTitleDesc,

        //Staff
        StaffNameAsc,
        StaffNameDesc,
        StaffSurnameAsc,
        StaffSurnameDesc,
        StaffPositionAsc,
        StaffPositionDesc,

        //Pricelist
        PricelistDiscIdAsc,
        PricelistDiscIdDesc,
        PricelistPriceAsc,
        PricelistPriceDesc,

        //Producers
        ManufacturerAsc,
        ManufacturerDesc,
        CounrtyAsc,
        CounrtyDesc,

        //Takings
        TakingsDiskIdAsc,
        TakingsDiskIdDesc,
    }

    public class SortViewModel
    {
        //Genre
        public SortState GenreTitleSort { get; set; }

        //Clientele
        public SortState ClientNameSort { get; set; }
        public SortState ClientSurnameSort { get; set; }

        //Disks
        public SortState DiskTitleSort { get; set; }
        public SortState DiskCreationYearSort { get; set; }
        public SortState DiskProducerSort { get; set; }
        public SortState MainActorSort { get; set; }
        public SortState RecordingDateSort { get; set; }
        public SortState DiskGenreIdSort { get; set; }
        public SortState DiskTypeIdSort { get; set; }

        //Producer
        public SortState ProducerManufacturerSortState { get; set; }
        public SortState ProducerCountrySortState { get; set; }

        //Types
        public SortState TypeTitleSort { get; set; }

        //Staff
        public SortState StaffSurnameSort { get; set; }
        public SortState StaffNameSort { get; set; }
        public SortState StaffPositionSort { get; set; }

        //Positions
        public SortState PositionNameSort { get; set; }

        //Pricelist
        public SortState PricelistDiskIdSort { get; set; }
        public SortState PricelistPriceSort { get; set; }

        //Takings 
        public SortState TakingsSort { get; set; }

        public SortState CurrentState { get; set; }

        public SortViewModel(SortState state)
        {
            //Genre
            GenreTitleSort = state == SortState.GenreTitleAsc ? SortState.GenreTitleDesc : SortState.GenreTitleAsc;
            CurrentState = state;

            //Clientele
            ClientNameSort = state == SortState.ClientNameAsc ? SortState.ClientNameDesc : SortState.ClientNameAsc;
            CurrentState = state;

            ClientSurnameSort = state == SortState.ClientSurnameAsc ? SortState.ClientSurnameDesc : SortState.ClientSurnameAsc;
            CurrentState = state;

            //Disks
            DiskTitleSort = state == SortState.DiskTitleAsc ? SortState.DiskTitleDesc : SortState.DiskTitleAsc;
            CurrentState = state;

            DiskCreationYearSort = state == SortState.DiskCreationYearAsc ? SortState.DiskCreationYearDesc : SortState.DiskCreationYearAsc;
            CurrentState = state;

            DiskProducerSort = state == SortState.DiskProducerAsc ? SortState.DiskProducerDesc : SortState.DiskProducerAsc;
            CurrentState = state;

            MainActorSort = state == SortState.MainActorAsc ? SortState.MainActorDesc : SortState.MainActorAsc;
            CurrentState = state;

            RecordingDateSort = state == SortState.RecordingDateAsc ? SortState.RecordingDateDesc : SortState.RecordingDateAsc;
            CurrentState = state;

            DiskGenreIdSort = state == SortState.GenreIdAsc ? SortState.GenreIdDesc : SortState.GenreIdAsc;
            CurrentState = state;

            DiskTypeIdSort = state == SortState.TypeIdAsc ? SortState.TypeIdDesc : SortState.TypeIdAsc;
            CurrentState = state;

            //Producers
            ProducerCountrySortState = state == SortState.CounrtyAsc ? SortState.CounrtyDesc : SortState.CounrtyAsc;
            CurrentState = state;

            ProducerManufacturerSortState = state == SortState.ManufacturerAsc ? SortState.ManufacturerDesc : SortState.ManufacturerAsc;
            CurrentState = state;

            //Types
            TypeTitleSort = state == SortState.TypeTitleAsc ? SortState.TypeTitleDesc : SortState.TypeTitleAsc;
            CurrentState = state;

            //Staff
            StaffNameSort = state == SortState.StaffNameAsc ? SortState.StaffNameDesc : SortState.StaffNameAsc;
            CurrentState = state;

            StaffSurnameSort = state == SortState.StaffSurnameAsc ? SortState.StaffSurnameDesc : SortState.StaffSurnameAsc;
            CurrentState = state;

            StaffPositionSort = state == SortState.PositionTitleAsc ? SortState.PositionTitleDesc : SortState.PositionTitleAsc;
            CurrentState = state;

            //Positions
            PositionNameSort = state == SortState.PositionTitleAsc ? SortState.PositionTitleDesc : SortState.PositionTitleAsc;
            CurrentState = state;

            //Pricelist
            PricelistDiskIdSort = state == SortState.PricelistDiscIdAsc ? SortState.PricelistDiscIdDesc : SortState.PricelistDiscIdAsc;
            CurrentState = state;

            PricelistPriceSort = state == SortState.PricelistPriceAsc ? SortState.PricelistPriceDesc : SortState.PricelistPriceAsc;
            CurrentState = state;

            //Takings
            TakingsSort = state == SortState.TakingsDiskIdAsc ? SortState.TakingsDiskIdDesc : SortState.TakingsDiskIdAsc;
            CurrentState = state;
        }
    }
}