import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { TaskItem } from '../../models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.scss',
})
export class TaskForm implements OnChanges {
  @Input() task: TaskItem | null = null;
  @Output() save = new EventEmitter<TaskItem>();
  @Output() cancel = new EventEmitter<void>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      details: [''],
      status: [0],
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['task']) {
      if (this.task) {
        this.form.patchValue(this.task);
      } else {
        this.form.reset({ title: '', details: '', status: 0 });
      }
    }
  }

  onSubmit() {
    if (this.form.valid) {
      this.save.emit(this.form.value as TaskItem);
    }
  }
}
