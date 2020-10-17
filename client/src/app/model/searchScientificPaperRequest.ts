export class SearchScientificPaperRequest {
    public magazineTitle: string ;
    public title: string;
    public authorFirstname: string;
    public authorLastname: string;
    public keyword: string;
    public scientificField: string;
    public content: string;
    public operation: string;
    public moreLikeThisEnabled: boolean;
    public moreLikeThisQuery: string;
}