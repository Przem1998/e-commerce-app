import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IType } from 'src/app/shared/models/productType';
import { ISystemType } from 'src/app/shared/models/systemType';
import { ShopService } from 'src/app/shop/shop.service';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

  constructor(private shopService:ShopService, private router:Router, private route:ActivatedRoute, private fb:FormBuilder, private dashboardService:DashboardService, private toastrService: ToastrService) { }
  systems: ISystemType[];
  types: IType[];
  categoriesGroup:FormGroup
  ngOnInit(): void {
    this.getSystems();
    this.getTypes();
    this.initialCategoriesInputs();
  }
  getSystems(){
    this.shopService.getSystems().subscribe(response =>{
      this.systems = response
    }, error =>{
      console.log(error);
    });
  }
  getTypes(){
    this.shopService.getTypes().subscribe(response =>{
      this.types =  response;
    }, error =>{
      console.log(error);
    });
  }
  initialCategoriesInputs(){
  this.categoriesGroup=this.fb.group({
    systemTypeValue: new FormControl('',Validators.required),
    productTypeValue: new FormControl('',Validators.required)
  })
  }
  reload(){
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';
        this.router.navigate(['./'], {
          relativeTo:this.route
        })
  }
  addProductType(){
   for(let i=0; i<this.types.length; i++){
      if(this.types[i].name==this.categoriesGroup.get('productTypeValue').value) this.toastrService.error("Wprowadzony typ produktu już istnieje");
      else if(i== this.types.length-1){
        if(this.types[i].name==this.categoriesGroup.get('productTypeValue').value) this.toastrService.error("Wprowadzony typ produktu już istnieje");
        else{
        console.log(this.categoriesGroup.get('productTypeValue').value);
          this.dashboardService.addProductType(this.mapToProductType(this.types.length+1,this.categoriesGroup.get('productTypeValue').value)).subscribe((res: IType)=>{
            this.toastrService.success("Dodano "+ res.name);
            this.reload();
          })
        }
      }
    }
  }
  mapToProductType(typeId:number,productType:string): IType{
    return {
      id:typeId,
      name:productType
    }
  }
  mapToSystemType(systemTypeId:number,systemType:string): ISystemType{
    return {
      id:systemTypeId,
      name:systemType
    }
  }

  addSystemType(){   
    for(let i=0; i<this.systems.length; i++){
      if(this.systems[i].name==this.categoriesGroup.get('systemTypeValue').value) this.toastrService.error("Wprowadzony system instalacyjny już istnieje");
      else if(i== this.systems.length-1){
        if(this.systems[i].name==this.categoriesGroup.get('systemTypeValue').value) this.toastrService.error("Wprowadzony typ produktu już istnieje");
        else{
        console.log(this.categoriesGroup.get('systemTypeValue').value);
          this.dashboardService.addSystemType(this.mapToSystemType(this.systems.length+1,this.categoriesGroup.get('systemTypeValue').value)).subscribe((res: ISystemType)=>{
            this.toastrService.success("Dodano "+ res.name);
            this.reload();
          })
        }
      }
    }
  }

  deleteSystemType(systemTypeId:number){
    this.dashboardService.checkCanDeleteSystemType(systemTypeId).subscribe((res:boolean) =>{
      if(res){
      this.dashboardService.deleteSystemType(systemTypeId).subscribe(res=>{
        
      },error=>{
        this.reload();
        this.toastrService.success("System instalacyjny usunięto pomyślnie");
      });
   
      }
      else this.toastrService.error("Ten rodzaj kategorii jest w użyciu");
    })
  }
  
 
  deleteProductType(productTypeId:number){
    this.dashboardService.checkCanDeleteProductType(productTypeId).subscribe((res:boolean) =>{
      if(res){
      this.dashboardService.deleteProductType(productTypeId).subscribe(res=>{
        
      },error=>{
        this.reload();
        this.toastrService.success("Typ kategorii usunięto pomyślnie");
      });
   
      }
      else this.toastrService.error("Ten rodzaj kategorii jest w użyciu");
    })
  }
}
