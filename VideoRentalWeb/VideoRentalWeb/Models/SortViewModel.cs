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

        //Pricelist
        PricelistDiscIdAsc,
        PricelistDiscIdDesc,

        //Producers
        ManufacturerAsc,
        ManufacturerDesc,

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

        //Disks
        public SortState DiskTitleSort { get; set; }

        public SortState DiskCreationYearSort { get; set; }
        public SortState DiskProducerSort { get; set; }
        public SortState MainActorSort { get; set; }
        public SortState RecordingDateSort { get; set; }
        public SortState DiskGenreIdSort { get; set; }
        public SortState DiskTypeIdSort { get; set; }

        //Types
        public SortState TypeTitleSort { get; set; }

        //Staff
        public SortState StaffNameSort { get; set; }

        //Positions
        public SortState PositionNameSort { get; set; }

        //Pricelist
        public SortState PricelistSort { get; set; }

        //Takings 
        public SortState TakingsSort { get; set; }



        public SortState CurrentState { get; set; }

        public SortViewModel(SortState state)
        {
            //Genre
            GenreTitleSort = state == SortState.GenreTitleAsc ? SortState.GenreTitleDesc : SortState.GenreTitleAsc;
            CurrentState = state;

            //Clientele
            ClientNameSort = state ==SortState.ClientNameAsc? SortState.ClientNameDesc: SortState.ClientNameAsc;

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

            //Types
            TypeTitleSort = state == SortState.TypeTitleAsc ? SortState.TypeTitleDesc : SortState.TypeTitleAsc;
            CurrentState = state;

            //Staff
            StaffNameSort = state == SortState.StaffNameAsc? SortState.StaffNameDesc : SortState.StaffNameAsc;

            //Positions
            PositionNameSort = state ==SortState.PositionTitleAsc? SortState.PositionTitleDesc : SortState.PositionTitleAsc;

            //Pricelist
            PricelistSort = state ==SortState.PricelistDiscIdAsc? SortState.PricelistDiscIdDesc: SortState.PricelistDiscIdAsc;

            //Takings
            TakingsSort = state ==SortState.TakingsDiskIdAsc? SortState.TakingsDiskIdDesc: SortState.TakingsDiskIdAsc;
        }
    }
}