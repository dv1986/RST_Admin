import { Component, OnInit } from '@angular/core';
import { DemoModuleService } from '../demo-service';

@Component({
    selector: 'app-code-generator',
    templateUrl: './code-generator.component.html',
    styleUrls: ['./code-generator.component.css']
})
export class CodeGeneratorComponent implements OnInit {
    public ProcedureName: string;
    public Parameters: any;
    public GeneratedCode: any;
    ProcedureError: string;
    constructor(private service: DemoModuleService) { }

    ngOnInit() {

    }
    GenerateCode(param) {
        this.GeneratedCode = null;
        if (param == 'G') {
            this.service.GenerateCode(this.ProcedureName, this.Parameters).subscribe(response => {
                debugger;
                if (response.state == 0) {
                    debugger;
                    this.GeneratedCode = response.data;
                }
            });
        }

        if (param == 'I') {
            this.service.GenerateFunction(this.ProcedureName, 'I').subscribe(response => {
                debugger;
                if (response.state == 0) {
                    debugger;
                    this.GeneratedCode = response.data;
                }
            });
        }

        if (param == 'U') {
            this.service.GenerateFunction(this.ProcedureName, 'U').subscribe(response => {
                debugger;
                if (response.state == 0) {
                    debugger;
                    this.GeneratedCode = response.data;
                }
            });
        }

        if (param == 'D') {
            this.service.GenerateFunction(this.ProcedureName, 'D').subscribe(response => {
                debugger;
                if (response.state == 0) {
                    debugger;
                    this.GeneratedCode = response.data;
                }
            });
        }
    }
    FetchColumns(params) {
        this.GeneratedCode == null;
        if (this.ProcedureName != null && this.ProcedureName != "") {
            this.ProcedureError = "";
            this.service.GetProcedureParameters(this.ProcedureName).subscribe(response => {
                if (response.state == 0) {
                    this.Parameters = response.data;
                    if (this.Parameters.length == 0) {
                        this.GenerateCode('G');
                    }
                }
            });
        }
        else {
            this.ProcedureError = "Procedure Name is required";
        }
    }
}
