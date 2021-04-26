export class GridAction {
    public GridAction() {

    }
    public actionId: number;
    public title: string;
    public cssClass: string;
    public isDisabled: any
    public onClick: any;
    public actionType = "button";
    public moduleName: string;
    public params: string;

    public requiresConfirmation: boolean=false;
    public confirmationPlacement: string="bottom";/*bottom,left,top,right*/
    public confirmationTitle: string="";
    public confirmationMessage: string="";
    public confirmationYesAction: any=null;
    public confirmationNoAction: any=null;

    
}
