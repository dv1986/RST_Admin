import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ViewChild, OnDestroy, Input, AfterViewInit } from '@angular/core';
import { GridFilterDirective } from './grid-filter.directive';
import { GridFilterItem } from './GridFilterItem';

@Component({
    selector: 'app-grid-filters',
    templateUrl: './grid-filters.component.html',
    styleUrls: ['./grid-filters.component.css']
})
export class GridFiltersComponent implements OnInit, AfterViewInit, OnDestroy {

    @Input() filterComponent: GridFilterItem;

    @ViewChild(GridFilterDirective) appFilter: GridFilterDirective;
    constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

    ngOnInit() {
        setTimeout( () => { this.loadComponent() }, 100 );
    }
    ngAfterViewInit() {

    }
    ngOnDestroy() {

    }
    loadComponent() {
        if (this.appFilter !== undefined) {
            if (this.filterComponent != null && this.filterComponent.component != null) {
                const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.filterComponent.component);

                const viewContainerRef = this.appFilter.viewContainerRef;
                viewContainerRef.clear();

                const componentRef = viewContainerRef.createComponent(componentFactory);
                componentRef.instance.parentComponent = this.filterComponent.parentComponent;
            }
        }
    }

}
