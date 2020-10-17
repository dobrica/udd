import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SearchFormComponent } from '../app/search-form/search-form.component'
import { NewScpaperComponent } from '../app/new-scpaper/new-scpaper.component';

const routes: Routes = [
  {
    path: 'scientificPapers/search',
    component: SearchFormComponent
  },
  {
    path: 'scientificPaper/add',
    component: NewScpaperComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
