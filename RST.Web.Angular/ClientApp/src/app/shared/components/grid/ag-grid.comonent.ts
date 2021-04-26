import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, AfterViewInit } from '@angular/core';
import { GridOptions, ColGroupDef, ColDef, CellValueChangedEvent, Column } from 'ag-grid-community';
import { GridInformation } from './grid-information';
import { Guid } from '../../services/guid';
import { Observable } from 'rxjs';
import { GridFilterItem } from './grid-filters/GridFilterItem';



@Component({
    selector: 'app-ag-grid',
    templateUrl: './ag-grid.comonent.html',
    styleUrls: ['./ag-grid.comonent.css'],
})
export class AgGridComponent implements OnInit, OnChanges, AfterViewInit {

    /******************************
   GRid Api Varaibles
   *******************************/
    private gridApi;
    private gridColumnApi;

    /********************************************************
    * Action Buttons on top of grid
   *********************************************************/
    @Input() gridActions = [];

    /*******************************************
    Grid Column Definition variable & Grid Option 
    ****************************************/
    @Input() options: GridOptions = <GridOptions>{};
    @Input() allowColumnResize: boolean = true;
    @Input() showRowIndex: boolean = true;
    @Input() showCheckBoxSelection: boolean = false;
    @Input() enableSummaryRow: boolean = false;
    @Input() columns: (ColDef | ColGroupDef)[];
    @Input() filterComponent: GridFilterItem = null;
    showFilter: boolean = false;
    filterButtonText: string = "Search Options";
    filterButtonClass: string = "";

    /***************************************************
    Default Text Font-size and Row Height 
    These must be initialized before data loading as it cannot be changed during runtime
    ***************************************************/
    @Input() fontSize: number = 11;
    @Input() rowHeight: number = 21;
    @Input() defaultColDef: any
    @Input() allowPaging: boolean = false;
    @Input() pagingSize: number = 200;
    @Input() cacheBlockSize: number = 200;


    /**************************************************************
    Data Binding Variables
    isServerModel: true if the grid will retrieve it's data from the server.
    data: In Memory data will be ignored if the isServerModel true
    dataSourceUrl : the web API url for retrieving the data from the server which is also responsible for caching the grid data
    filterQuery: the initial filter the Web Api takes for filtering the data before caching it. 
    *************************************************************/
    @Input() data: any = null;
    @Input() selectMode = "";
    private summaryColumns = null;

    @Output() ready: EventEmitter<any> = new EventEmitter();
    @Output() rowDoubleClick: EventEmitter<any> = new EventEmitter();
    @Output() rowClick: EventEmitter<any> = new EventEmitter();
    @Output() selectionChange: EventEmitter<any> = new EventEmitter();
    @Output() dataChangedUpdated: EventEmitter<any> = new EventEmitter();
    // @Output() dataLoaded: EventEmitter<any> = new EventEmitter();
    // @Output() filterChanged: EventEmitter<any> = new EventEmitter();
    @Output() dataChanged: EventEmitter<any> = new EventEmitter();
    @Output() dataUpdated: EventEmitter<any> = new EventEmitter();
    // @Output() fetchFirstRow: EventEmitter<any> = new EventEmitter();
    @Output() CellValueChanged: EventEmitter<any> = new EventEmitter();
    @Output() summaryUpdated: EventEmitter<any> = new EventEmitter();

    private gridUniqueId: any;
    @Input("gridInfo") _gridInfo: GridInformation = { id: "", updatedData: [], selectedData: [] };

    /**************************************************************
    Others
    *************************************************************/
    private editRowIndex = -1;
    private editColId = "";

    get gridInfo() {
        return this._gridInfo;
    }

    set gridInfo(val) {
        this._gridInfo = val;
        this.propagateChange(val);
    }

    propagateChange = (_: any) => { };

    constructor() {
        this.gridUniqueId = this.newGuid();
    }

    toggleFilter() {
        if (this.showFilter == true) {

            this.filterButtonText = "Search Options"
            this.showFilter = false;
            this.filterButtonClass = "";
        }
        else {
            this.filterButtonText = "Search Options"
            this.showFilter = true;
            this.filterButtonClass = "dropup";
        }
    }

    newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0,
                v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    ngOnInit() {
        //Setting Column Types
        if (!this.options.columnTypes)
            this.options.columnTypes = this.columnTypes;
        this.options.rowStyle = { 'font-size': `${this.fontSize}px` };


        this.options.getRowClass = function (params) {
            //Due to Order of events don't process the highlight of updated data until there is exactly data update in the list
            //As erasing happen when the render is complete on data-row changed
            if (params.context != null && params.context.thisComponent.gridInfo.updatedData.length > 0) {
                if (params.context.thisComponent.changedNodesIndex.indexOf(params.node.rowIndex) >= 0) {
                    return 'ag-row-modified';
                }
            }
        }
    }
    ngAfterViewInit() {
        if (!this.options.columnApi) return; //check the grid is initialized 
        var allColumns = this.options.columnApi.getAllGridColumns();
        for (var i = 0; i < allColumns.length; i++) {
            this.applyFormattingRules(allColumns[i]);
        }
    }

    ngOnChanges(changes: SimpleChanges): void {

        this.showRowIndex = true;
        if (changes.data && changes.data.firstChange) {


        }

        if (changes.columns && changes.columns.firstChange) {


        }

        if (changes.options && changes.options.firstChange) {
            if (this.options && this.options.columnDefs &&
                this.options.columnDefs.length > 0 &&
                this.options.columnDefs.filter(s => s.headerName == "#").length == 0) {
                if (this.showCheckBoxSelection || this.showRowIndex) {
                    let col: any = {
                        suppressSorting: true,
                        suppressFilter: true,
                        suppressMenu: true,
                        pinned: "left",
                        headerName: '#',
                        lockPosition: true,
                        cellClass: "text-center",
                        type: ["count"]
                    }
                    col["pinnedRowCellRenderer"] = "customPinnedRowRenderer";
                    col["pinnedRowCellRendererParams"] = { style: { "font-weight": "bold" } };
                    col["summaryAggFunc"] = "count";
                    col["summaryAggFormula"] = "";
                    col["columnFormat"] = "Number";

                    if (this.showCheckBoxSelection) {
                        col.checkboxSelection = true;
                    }

                    if (this.showRowIndex) {
                        col.valueGetter = function (params) {
                            if (params.node.rowPinned)
                                return params.data["index"];
                            return params.node.rowIndex + 1
                        };
                    }

                    this.options.columnDefs.unshift(col);
                    this.options.columnDefs.forEach(element => {
                        /*if (element.headerClass && element.headerClass.length > 0) {
                          (<ColGroupDef>element).children.forEach(def => {
                            (<ColDef>def).lockPosition = true;
                          });
                        }*/
                    });
                }
            }

        }

        this.updateSummaryRow(changes.data.currentValue);
    }

    updateSummaryRow(data) {
        this.enableSummaryRow = true;
        this.populateSummaryColumns(true);

        var summary = {};

        if (this.summaryColumns != null && this.summaryColumns.length > 0) {
            var summaryRecord = {};
            var summary = {};

            for (var i = 0; i < this.summaryColumns.length; i++) {
                summaryRecord[this.summaryColumns[i].Field] = 0;
                var fieldName = this.summaryColumns[i].ColumnName;
                // var ind = fieldName.indexOf('$');
                // if (ind > 0)
                //     fieldName = this.summaryColumns[i].ColumnName.substring(ind + 1);
                this.summaryColumns[i]['FieldName'] = fieldName;
                summary[fieldName] = 0;
            }
            for (var i = 0; i < this.summaryColumns.length; i++) {
                let sum = 0;
                for (var j = 0; j < data.length; j++) {
                    sum += Number(data[j][this.summaryColumns[i].Field]);
                }
                summaryRecord[this.summaryColumns[i].Field] += sum;
                summary[this.summaryColumns[i].FieldName] += sum;
            }
            if (data.length > 0) {

                for (var i = 0; i < this.summaryColumns.length; i++) {
                    if (this.summaryColumns[i].Operation == "avg") {
                        summaryRecord[this.summaryColumns[i].Field] = summaryRecord[this.summaryColumns[i].Field] / data.length;
                        summary[this.summaryColumns[i].FieldName] = summary[this.summaryColumns[i].Field] / data.length;

                    }
                    else if (this.summaryColumns[i].Operation == "count") {
                        summaryRecord[this.summaryColumns[i].Field] = data.length;
                        summary[this.summaryColumns[i].FieldName] = data.length;
                    }
                    else if (this.summaryColumns[i].Operation == "sum") {
                        summaryRecord[this.summaryColumns[i].Field] = summaryRecord[this.summaryColumns[i].Field];
                        summary[this.summaryColumns[i].FieldName] = summary[this.summaryColumns[i].Field];
                    }
                }
            }
            for (var i = 0; i < this.summaryColumns.length; i++) {
                if (this.summaryColumns[i].Field != "index") {
                    summaryRecord[this.summaryColumns[i].Field] = summaryRecord[this.summaryColumns[i].Field].toFixed(2);
                    //summary[this.summaryColumns[i].FieldName] = summary[this.summaryColumns[i].FieldName].toFixed(2);
                }
            }
            for (var i = 0; i < this.summaryColumns.length; i++) {
                if (this.summaryColumns[i].Operation == "custom")
                    summaryRecord[this.summaryColumns[i].Field] = eval(this.summaryColumns[i].Formula);
            }
            this.notifySummaryRowUpdate(summaryRecord);
        }
    }

    populateSummaryColumns1(force: boolean = false) {

        if ((this.enableSummaryRow && this.options != null && this.options.columnApi != null) && (this.summaryColumns == null || force)) {
            this.summaryColumns = [];
            this.options.columnApi.getAllColumns().forEach(element => {
                let colDef: ColDef = element.getColDef();
                if (colDef['field'] != null) {
                    if (colDef['summaryAggFunc'] != null) {
                        this.summaryColumns.push({ Field: colDef['field'], ColumnName: colDef['colkey'], Operation: colDef['summaryAggFunc'], Formula: colDef['summaryAggFormula'] });
                    }
                }
            });
            this.summaryColumns.push({ ColumnName: 'index', Field: 'index', Operation: 'count' });
        }
    }

    populateSummaryColumns(force: boolean = false) {

        if ((this.enableSummaryRow && this.options != null && this.options.columnApi != null) && (this.summaryColumns == null || force)) {
            this.summaryColumns = [];
            this.options.columnApi.getAllColumns().forEach(element => {
                let colDef: any = element.getColDef();
                if (colDef.aggFunc !== undefined) {
                    this.summaryColumns.push({ Field: colDef['field'], ColumnName: colDef['headerName'], Operation: colDef.aggFunc });
                }
            });
            this.summaryColumns.push({ ColumnName: 'index', Field: 'index', Operation: 'count' });
        }
    }

    notifySummaryRowUpdate(summaryRecord) {
        this.options.api.setPinnedBottomRowData([summaryRecord]);
        this.summaryUpdated.emit(summaryRecord);
    }


    private columnTypes = {
        editableColumn: { cellClass: "cellEditable", editable: true }
    }//

    onGridReady(params) {
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;

        this.options.columnApi.getAllColumns().forEach(function (column: Column) {
            column.getColDef().sortingOrder = ["asc", "desc"];
            let def: ColDef = column.getColDef();
            if (def.type) {

            }
            else {
                column.getColDef().cellClass = "text-center";
            }
            if (column.getParent()) {
                if (typeof column.getParent().getDefinition().headerClass != 'undefined' && column.getParent().getDefinition().headerClass.length > 0) {

                    //column.getColDef().suppressMovable = true;
                    column.getColDef().lockVisible = true;
                    //column.getColDef().lockPosition = true;
                }
            }
        });
        if (this.options.columnApi != null) {
            let columns = this.options.columnApi.getAllColumns();
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].getColDef()["colkey"] == null || columns[i].getColDef()["colkey"] == "")
                    columns[i].getColDef()["colkey"] = columns[i].getColDef()["headerName"];
            }
        }
        this.options.context = { thisComponent: this };
        this.setSummaryRowRendererFramework()

        //this.initializeWindowsModulesLinks();

        this.autoSizeAllColumns();



        this.ready.emit();
    }

    autoSizeAllColumns() {
        if (!this.options.columnApi) return; //check the grid is initialized 
        this.options.columnApi.autoSizeAllColumns();
    }

    autoSizeColumns() {
        if (!this.options.columnApi) return; //check the grid is initialized 
        var allColumnIds = [];
        this.options.columnApi.getAllColumns().forEach(function (column) {
            allColumnIds.push(column.getId());
        });
        this.options.columnApi.autoSizeColumns(allColumnIds);
    }

    setSummaryRowRendererFramework() {
        this.options.columnApi.getAllColumns().forEach(element => {
            let columnDef: ColDef = element.getColDef();
            if (columnDef["summaryAggFunc"] != null) {
                columnDef["pinnedRowCellRenderer"] = "customPinnedRowRenderer";
                columnDef["pinnedRowCellRendererParams"] = { style: { "font-weight": "bold" } };
                //columnDef["autoHeight"] = true;
            }
        });
    }

    //#region Editing Handling
    protected undoHistory: any = [];
    @Input() keyField: string = "";
    @Input() UpdateApiUrl: string = ""
    private changedNodesIndex: number[] = [];
    // onCellValueChanged(params: CellValueChangedEvent) {
    onCellValueChanged(params: any) {


        if (params.newValue != params.oldValue) {
            let currentEditColumn = null;
            this.CellValueChanged.emit(params)

            if (this.editRowIndex > -1 && this.editColId != "") //Need to update first then Edit adjacent column
            {
                currentEditColumn = this.gridApi.getEditingCells();
                console.log(currentEditColumn, "| Stopping Editing")
                this.gridApi.stopEditing();
            }

            let colDef = params.column.getColDef();
            if (colDef.type !== undefined) {
                if (colDef.type.filter(s => s == "numberColumn").length > 0) {
                    params.data[colDef.field] = Number(params.data[colDef.field]);
                }
                else if (colDef.type.filter(s => s == "Currency").length > 0) {
                    params.data[colDef.field] = Number(params.data[colDef.field]);
                }
                else if (colDef.type.filter(s => s == "Percentage Decimal").length > 0) {
                    params.data[colDef.field] = Number(params.data[colDef.field]);
                }
            }

            //Store Updated Data based on the key
            // first check if the data is stored or not before         
            if (this.keyField != "") {
                let found = false;
                this.gridInfo.updatedData.forEach(element => {
                    if (element[this.keyField] == params.data[this.keyField]) {
                        element = params.data;
                        found = true;
                    }
                });
                if (!found) {

                    this.gridInfo.updatedData.push(params.data);
                }
                //Used to highlight changed rows.
                this.changedNodesIndex.push(params.node.rowIndex);
                params.node.setDataValue(this.keyField, params.data[this.keyField]);
            }

        }
        this.editRowIndex = -1;
        this.editColId = "";
    }

    onUndoClick(params) {
        let historyId = (<any>params.target).id;
        let selectedItem = null;
        this.undoHistory.forEach(element => {

            if (element.id == historyId) {
                element.node.data[element.columnId] = element.oldValue;
                //element.node.setData(element.node.data);    
                selectedItem = element;
            }
        });
        if (selectedItem) { //Update Data
            // this.updateData(selectedItem.node.data).subscribe(response => {
            // if (response.state == 0) { //Update Sucess 
            // selectedItem.node.setData(response.data);
            // this.updateCache(response.data).subscribe(result => { });
            let index = this.undoHistory.indexOf(selectedItem, 0);
            if (index > -1) {
                this.undoHistory.splice(index, 1);
            }
        }
        // });

        // }
    }

    // private updateData(data): Observable<any> {
    //     let backendUrl = `${environment.baseApiUrl}/${this.UpdateApiUrl}`;
    //     return this.http.post<any>(backendUrl, data);
    // }

    //#endregion

    onRowDblClick(params) {
        this.rowDoubleClick.emit(params.data);
    }

    onViewportChanged(params) {

    }

    onBodyScroll(params) {

    }

    onSelectionChanged(params) {
        let selectedData = this.gridApi.getSelectedNodes();
        this.gridInfo.selectedData = this.gridApi.getSelectedRows();
        this.selectionChange.emit(selectedData);
    }

    onColumnVisible(params) {

    }

    onColumnEverything(params) {

    }

    onColumnResize(params) {

    }

    onRowClick(params) {
        this.rowClick.emit(params);
    }

    oncellEditingStarted(params) {

    }

    onFilterChanged(params) {

    }

    onRowDataChanged(params) {
        this.dataChanged.emit(params);
    }

    // #region Apply Dynamic Styling
    applyFormattingRules(column) {
        let columnDef = column.getColDef();
        if (columnDef.type !== undefined) {
            if (columnDef.type.filter(s => s == "Date & Time").length > 0) {
                columnDef.valueFormatter = this.dateTimeFormatter;
                this.applycellClass(columnDef, "text-center");
                columnDef.filter = "agDateColumnFilter";
                columnDef.tooltip = this.dateTooltipFormatter;
            }
            else if (columnDef.type.filter(s => s == "Time").length > 0) {
                columnDef.valueFormatter = this.timeFormatter;
                this.applycellClass(columnDef, "text-center");
                columnDef.filter = "agDateColumnFilter";
                columnDef.tooltip = this.dateTooltipFormatter;
            }
            else if (columnDef.type.filter(s => s == "Date").length > 0) {
                columnDef.valueFormatter = this.dateFormatter;
                this.applycellClass(columnDef, "text-center");
                columnDef.filter = "agDateColumnFilter";
            }
            else if (columnDef.type.filter(s => s == "Email").length > 0) {
                this.applycellClass(columnDef, "text-center");
                columnDef.cellRenderer = function (params) {
                    var targetURL = "https://outlook.office.com/mail/deeplink/compose?to=" + params.value;
                    return '<a href="javascript:void();" onclick="window.open(\'' + targetURL + '\', \'_blank\', \'toolbar=yes,scrollbars=yes,resizable=yes,top=150,left=300,width=1000,height=800\')">' + params.value + '</a>';
                }
            }
            else if (columnDef.type.filter(s => s == "Phone").length > 0) {
                this.applycellClass(columnDef, "text-center");
                columnDef.valueFormatter = this.phoneNumberFormatter;
            }
            else if (columnDef.type.filter(s => s == "Currency").length > 0) {
                columnDef.valueFormatter = this.currencyPriceFormatter;
                this.applycellClass(columnDef, "text-right");
                columnDef.valueParser = this.numberParser;
                columnDef.filter = "agNumberColumnFilter";
                columnDef.filterParams = { newRowsAction: "keep", filterOptions: ["equals", "notEqual", "lessThan", "greaterThan"] };
            }
            else if (columnDef.type.filter(s => s == "numberColumn").length > 0) {
                this.applycellClass(columnDef, "text-right");
                columnDef.valueParser = this.numberParser;
                columnDef.valueFormatter = this.numberFormatter;
                columnDef.filter = "agNumberColumnFilter";
                columnDef.filterParams = { newRowsAction: "keep", filterOptions: ["equals", "notEqual", "lessThan", "greaterThan"] };
            }
            else if (columnDef.type.filter(s => s == "Decimal").length > 0) {
                this.applycellClass(columnDef, "text-right");
                columnDef.valueFormatter = this.fixedPointFormatter;
                columnDef.valueParser = this.numberParser;
                columnDef.filter = "agNumberColumnFilter";
                columnDef.filterParams = { newRowsAction: "keep", filterOptions: ["equals", "notEqual", "lessThan", "greaterThan"] };
            }
            else if (columnDef.type.filter(s => s == "Decimal4").length > 0) {
                this.applycellClass(columnDef, "text-right");
                columnDef.valueFormatter = this.fixedPointFormatter4;
                columnDef.valueParser = this.numberParser;
                columnDef.filter = "agNumberColumnFilter";
                columnDef.filterParams = { newRowsAction: "keep", filterOptions: ["equals", "notEqual", "lessThan", "greaterThan"] };
            }
            else if (columnDef.type.filter(s => s == "Percentage Number").length > 0) {
                columnDef.valueFormatter = this.percentageIntegerFormatter;
                this.applycellClass(columnDef, "text-right");
                columnDef.valueParser = this.numberParser;
                columnDef.filter = "agNumberColumnFilter";
                columnDef.filterParams = { newRowsAction: "keep", filterOptions: ["equals", "notEqual", "lessThan", "greaterThan"] };
            }
            else if (columnDef.type.filter(s => s == "Percentage Decimal").length > 0) {
                columnDef.valueFormatter = this.percentageFormatter;
                this.applycellClass(columnDef, "text-right");
                columnDef.valueParser = this.numberParser;
                columnDef.filter = "agNumberColumnFilter";
                columnDef.filterParams = { newRowsAction: "keep", filterOptions: ["equals", "notEqual", "lessThan", "greaterThan"] };
            }
        }
    }
    applyStylingRules(columnDef, metaData) {
        if (metaData["dataType"] == "bigint" || metaData["dataType"] == "int" || metaData["dataType"] == "numeric" || metaData["dataType"] == "bigint" || metaData["dataType"] == "smallint" || metaData["dataType"] == "tinyint") {
            this.applycellClass(columnDef, "text-center");
        }
        else if (metaData["dataType"].startsWith("decimal") || metaData["dataType"] == "float" || metaData["dataType"] == "money" || metaData["dataType"] == "real") {
            this.applycellClass(columnDef, "text-right");
        }
        else if (metaData["dataType"].indexOf("char") >= 0) {
            var charLength = 0;

            if (metaData["dataType"].indexOf("(") >= 0) {
                charLength = metaData["dataType"].substring(metaData["dataType"].indexOf("(") + 1, metaData["dataType"].indexOf(")"))
            }
            var cssClass = "text-center";
            if (charLength > 5)
                cssClass = "text-left";
            this.applycellClass(columnDef, cssClass);
        }
        else if (metaData["dataType"] == "date") {
            if (metaData["columnFormat"] == null || metaData["columnFormat"] == "") {
                if (columnDef.valueFormatter != null) {
                    columnDef.valueFormatter = this.dateFormatter;
                }
                columnDef.filter = "agDateColumnFilter";
                this.applycellClass(columnDef, "text-center");
            }
        }
        else if (metaData["dataType"].startsWith("datetime")) {
            if (metaData["columnFormat"] == null || metaData["columnFormat"] == "") {
                if (columnDef.tooltip != null) {
                    columnDef.tooltip = this.dateTooltipFormatter;
                }
                if (columnDef.valueFormatter != null) {
                    columnDef.valueFormatter = this.dateFormatter;
                }
                columnDef.filter = "agDateColumnFilter";
                this.applycellClass(columnDef, "text-center");
            }
        }
        else if (metaData["dataType"] == "time") {
            this.applycellClass(columnDef, "text-center");
        }

        else {
            this.applycellClass(columnDef, "text-center");
        }
    }

    applycellClass(columnDef, cssClass) {
        if (columnDef.cellClass != null) {
            columnDef.cellClass = columnDef.cellClass.replace("text-center", " ");
            columnDef.cellClass = columnDef.cellClass.replace("text-left", " ");
            columnDef.cellClass = columnDef.cellClass.replace("text-right", " ");
            columnDef.cellClass = columnDef.cellClass.replace("number-cell", " ");
            columnDef.cellClass += " " + cssClass;
        }
        else
            columnDef.cellClass = cssClass;
    }
    // #endregion


    //#region Helper Functions
    phoneNumberFormatter(params) {
        if (params.value != null) {
            return params.value.replace(/(\d{4})(\d{3})(\d{3})/, "$1-$2-$3");
        }
        return params.value;
    }
    binCodeFormatter(params) {
        if (params.value != null) {
            return params.value.replace(/(\d{2})(\d{2})(\d{2})(\d{2})/, "$1-$2-$3-$4")

        }
        return params.value;
    }
    decimalFormatter(params) {

        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 2).toFixed(2).toString();
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            return ZeroFormatting;
        }
        return params.value;
    }
    currencyPriceFormatter(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 2).toFixed(2);
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            if (ZeroFormatting != "")
                return "\x24" + ZeroFormatting.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
            else
                return ZeroFormatting;
        }
        return params.value;
    }

    currencyCostFormatter(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 2).toFixed(2);
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            if (ZeroFormatting != "")
                return "\x24" + ZeroFormatting.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
            else
                return ZeroFormatting;
        }
        return params.value;
    }

    fixedPointFormatter(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 2).toFixed(2);
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            if (ZeroFormatting != "")
                return ZeroFormatting.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
            else
                return ZeroFormatting;
        }
        return params.value;
    }
    fixedPointFormatter4(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 4).toFixed(4).toString();
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            return ZeroFormatting;
        }
        return params.value;
    }
    numberFormatter(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 0);
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            if (ZeroFormatting != "")
                return ZeroFormatting.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
            else
                return ZeroFormatting;
        }
        return params.value;
    }
    percentageFormatter(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 2).toFixed(2)
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            if (ZeroFormatting != "")
                return ZeroFormatting.toString() + "\x25";
            else
                return ZeroFormatting;
        }
        return params.value;
    }
    dateFormatter(params) {
        if (params.value != null && params.value != "") {
            var date = null;
            if (params.value.indexOf("T") !== -1) {
                date = new Date(params.value);
            }
            else {
                var dateParts = params.value.split("/");
                if (dateParts.length > 2) {
                    var parts = dateParts[2].split(" ");
                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                }

            }
            if (date != null && date instanceof Date) {
                if (date.getHours() >= 12)
                    return `${pad(date.getDate(), 2)}-${pad(date.getMonth() + 1, 2)}-${date.getFullYear().toString().substr(-2)}`;
                else
                    return `${pad(date.getDate(), 2)}-${pad(date.getMonth() + 1, 2)}-${date.getFullYear().toString().substr(-2)}`;
            }
        }
        return params.value;
    }
    timeFormatter(params) {
        if (params.value != null && params.value != "") {
            var date = null;
            if (params.value.indexOf("T") !== -1) {
                date = new Date(params.value);
            }
            else {
                var dateParts = params.value.split("/");
                if (dateParts.length > 2) {
                    var parts = dateParts[2].split(" ");
                    if (parts.length > 1) {
                        var timeparts = parts[1].split(":");

                        if (timeparts.length > 2) {
                            if (parts.length > 2) {
                                if (parts[2] == "AM")
                                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0], timeparts[1]);
                                else
                                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0] + 12, timeparts[1]);
                            }
                            else
                                date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0], timeparts[1]);
                        }
                        else
                            date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                    }
                    else
                        date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                }

            }
            if (date != null && date instanceof Date) {
                if (date.getHours() >= 12)
                    return `${pad(date.getHours() - 12, 2)}.${pad(date.getMinutes(), 2)} PM`;
                else
                    return `${pad(date.getHours(), 2)}.${pad(date.getMinutes(), 2)} AM`;
            }
        }
        return params.value;
    }
    dateTimeFormatter(params) {
        if (params.value != null && params.value != "") {
            var date = null;
            if (params.value.indexOf("T") !== -1) {
                date = new Date(params.value);
            }
            else {
                var dateParts = params.value.split("/");
                if (dateParts.length > 2) {
                    var parts = dateParts[2].split(" ");
                    if (parts.length > 1) {
                        var timeparts = parts[1].split(":");

                        if (timeparts.length > 2) {
                            if (parts.length > 2) {
                                if (parts[2] == "AM")
                                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0], timeparts[1]);
                                else
                                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0] + 12, timeparts[1]);
                            }
                            else
                                date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0], timeparts[1]);
                        }
                        else
                            date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                    }
                    else
                        date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                }

            }
            if (date != null && date instanceof Date) {
                if (date.getHours() >= 12)
                    return `${pad(date.getDate(), 2)}-${pad(date.getMonth() + 1, 2)}-${date.getFullYear().toString().substr(-2)} ${pad(date.getHours() - 12, 2)}.${pad(date.getMinutes(), 2)} PM`;
                else
                    return `${pad(date.getDate(), 2)}-${pad(date.getMonth() + 1, 2)}-${date.getFullYear().toString().substr(-2)} ${pad(date.getHours(), 2)}.${pad(date.getMinutes(), 2)} AM`;
            }
        }
        return params.value;
    }
    dateTooltipFormatter(params) {
        if (params.value != null && params.value != "") {
            var date = null;
            if (params.value.indexOf("T") !== -1) {
                date = new Date(params.value);
            }
            else {
                var dateParts = params.value.split("/");
                if (dateParts.length > 2) {
                    var parts = dateParts[2].split(" ");
                    if (parts.length > 1) {
                        var timeparts = parts[1].split(":");

                        if (timeparts.length > 2) {
                            if (parts.length > 2) {
                                if (parts[2] == "AM")
                                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0], timeparts[1], timeparts[2]);
                                else
                                    date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0] + 12, timeparts[1], timeparts[2]);
                            }
                            else
                                date = new Date(parts[0], dateParts[1] - 1, dateParts[0], timeparts[0], timeparts[1], timeparts[1]);
                        }
                        else
                            date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                    }
                    else
                        date = new Date(parts[0], dateParts[1] - 1, dateParts[0]);
                }

            }
            if (date != null && date instanceof Date) {
                if (date.getHours() >= 12)
                    return `${pad(date.getDate(), 2)}-${pad(date.getMonth() + 1, 2)}-${date.getFullYear().toString().substr(-2)} ${pad(date.getHours() - 12, 2)}:${pad(date.getMinutes(), 2)}:${pad(date.getSeconds(), 2)} PM`;
                else
                    return `${pad(date.getDate(), 2)}-${pad(date.getMonth() + 1, 2)}-${date.getFullYear().toString().substr(-2)} ${pad(date.getHours(), 2)}:${pad(date.getMinutes(), 2)}:${pad(date.getSeconds(), 2)} AM`;
            }
        }
        return params.value;
    }
    percentageIntegerFormatter(params) {
        if (params.value != null) {
            var roundedVal = precisionRound(params.value, 2);
            var ZeroFormatting = formatZeroValue(roundedVal, params);
            if (ZeroFormatting != "")
                return ZeroFormatting.toString() + "\x25";
            else
                return ZeroFormatting;

        }
        return params.value;
    }
    numberParser(params) {

        if (params.newValue != null) {
            if (isNaN(params.newValue)) {
                return params.oldValue;
            }
            else {
                return Number(params.newValue);
            }
        }
        return params.oldValue;
    }
    zeroValueFormatter(params) {
        if (params.value != null) {
            return formatZeroValue(params.value, params);
        }
        return params.value;
    }
    // #endregion

}

function precisionRound(number, precision) {

    var factor = Math.pow(10, precision);
    return Math.round(number * factor) / factor;
}
function formatZeroValue(value, params) {
    if (params.colDef["allowZero"] != null && params.colDef["allowZero"] == "Y")
        return value;
    if (value == 0)
        return "";
    return value;
}

function pad(num, size) {
    var s = num + "";
    while (s.length < size) s = "0" + s;
    return s;
}