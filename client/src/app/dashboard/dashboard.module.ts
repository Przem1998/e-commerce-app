import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { OrdersComponent } from './orders/orders.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductsModule } from './products/products.module';
import { AdminLoginComponent } from './admin-login/admin-login.component';
import { AdminNavBarComponent } from './admin-nav-bar/admin-nav-bar.component';
import { CategoriesComponent } from './categories/categories.component';
import { OrdersModule } from './orders/orders.module';


@NgModule({
  declarations: [
    DashboardComponent,
    AdminLoginComponent,
    AdminNavBarComponent,
    CategoriesComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    CoreModule,
    SharedModule,
    ProductsModule,
    OrdersModule,
    DashboardRoutingModule,
  ]
})
export class DashboardModule { }
