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
        TypeDesc,

        //Clientele
        ClientSurnameAsc,

        ClientNameAsc,
        ClientMiddlenameAsc,
        ClientAddressAsc,
        ClientPhoneAsc,
        ClientPhoneDesc,
        ClientPassportAsc,
        ClientPassportDesc,
        ClientAddressDesc,
        ClientMiddlenameDesc,
        ClientNameDesc,
        ClientSurnameDesc,

        //Types
        TypeTitleAsc,

        TypeTitleDesc
    }

    public class SortViewModel
    {
        //Genre
        public SortState GenreTitleSort { get; set; }

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

        public SortState CurrentState { get; set; }

        public SortViewModel(SortState state)
        {
            //Genre
            GenreTitleSort = state == SortState.GenreTitleAsc ? SortState.GenreTitleDesc : SortState.GenreTitleAsc;
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
            //Types

            TypeTitleSort = state == SortState.TypeTitleAsc ? SortState.TypeTitleDesc : SortState.TypeTitleAsc;
            CurrentState = state;
        }
    }
}