import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IProduct } from 'src/app/shared/models/product';
import { IType } from 'src/app/shared/models/productType';
import { ISystemType } from 'src/app/shared/models/systemType';
import { IUser } from 'src/app/shared/models/user';
import { ShopService } from 'src/app/shop/shop.service';
import { DashboardService } from '../../dashboard.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {
  currentUser$: Observable<IUser>;
  breadcrumb$: Observable<any[]>;
  constructor(private shopService: ShopService, private dashboardService:DashboardService, private toastr:ToastrService, private router:Router, private route:ActivatedRoute, private accountService:AccountService, private fb:FormBuilder) { }

  ngOnInit(): void {
    this.createProductsForm();
    this.getSystems();
    this.getProductTypes();
    this.currentUser$=this.accountService.currentUser$
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
        productName: [null,
                      [Validators.required],
                      [this.validateProductNotTaken()]
                    ],
        productPrice: [null, [Validators.required, Validators.pattern("[0-9]+")]],
        description: [' '],
        systemType:[null, Validators.required],
        productType:[null, Validators.required],
        pictureUrl:[null]
      })
  }
  url="";
  selectedFile:File
  onFileSelectedListener(event){
    if(event.target.files){
      var reader = new FileReader();
      this.selectedFile=<File>event.target.files[0];
      reader.readAsDataURL(event.target.files[0]);
      reader.onload=(event:any)=>{
        this.url= event.target.result;
      }
    }
  }
  addProduct(){
      const fd = new FormData();
      fd.append('Files',this.selectedFile,this.selectedFile.name)
      this.dashboardService.deployImage(fd)
      .subscribe(res =>{
      },error=>{
            this.dashboardService.addProduct(this.mapToProduct( error.error.text)).subscribe( response =>{
          this.toastr.success('Pomyślnie dodano produkt');
        }, error => {
          console.log(error);
        });
      })
  }

  private mapToProduct(picturePath:string) : IProduct {
    return {
      id:1,
      name: this.productForm.controls['productName'].value,
      description: this.productForm.controls['description'].value,
      price:  +this.productForm.controls['productPrice'].value,
      pictureUrl: picturePath,
      productType:this.productForm.controls['productType'].value,
      systemType:this.productForm.controls['systemType'].value
    }
  }
  validateProductNotTaken() : AsyncValidatorFn {
    return control => {
      return timer(1500).pipe(
        switchMap(() => {
          if(!control.value){
            return of(null);
          }
          return this.dashboardService.checkProductExists(control.value).pipe(
            map(res => {
              return res ? {productExists: true} : null;
            })
          );
        })
      );
    }
  };
  logout(){
    this.accountService.logout();
  }
}
