import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IType } from 'src/app/shared/models/productType';
import { ISystemType } from 'src/app/shared/models/systemType';
import { ShopService } from 'src/app/shop/shop.service';
import { DashboardService } from '../../dashboard.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {

  constructor(private dashboardService: DashboardService, private shopService: ShopService, private fb:FormBuilder) { }

  ngOnInit(): void {
    this.createProductsForm();
    this.getSystems();
    this.getProductTypes();
  }
  systems: ISystemType[];
  productTypes: IType[];
  productForm: FormGroup
  getSystems(){
    this.shopService.getSystems().subscribe((systemType:ISystemType[])=>{
    this.systems=systemType;
    })
  }
  getProductTypes(){
    this.shopService.getTypes().subscribe((productType:IType[])=>{
    this.productTypes=productType
    })
  }
  createProductsForm(){
    this.productForm = this.fb.group({
        productName: [null, Validators.required],
        productPrice: [null, [Validators.required, Validators.pattern("[0-9]+")]],
        description: [null],
        systemType:[null, Validators.required],
        productType:[null, Validators.required]
      })
  }
  url="";
  onFileSelectedListener(event){
    if(event.target.files){
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      reader.onload=(event:any)=>{
        this.url= event.target.result;
      }
    }
  }
  addProduct(){
    
  }
}
