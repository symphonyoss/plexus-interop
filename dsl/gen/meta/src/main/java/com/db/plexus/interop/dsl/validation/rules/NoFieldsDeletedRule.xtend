/**
 * Copyright 2017-2020 Plexus Interop Deutsche Bank AG
 * SPDX-License-Identifier: Apache-2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package com.db.plexus.interop.dsl.validation.rules

import com.db.plexus.interop.dsl.gen.GenUtils
import com.google.inject.Inject
import org.eclipse.xtext.resource.XtextResourceSet
import static com.db.plexus.interop.dsl.validation.Issues.*;

class NoFieldsDeletedRule implements UpdateRule {

    val GenUtils genUtils;

    @Inject
    new(GenUtils genUtils) {
        this.genUtils = genUtils;
    }

    override getCode() '''message-field-deleted'''

    override validate(XtextResourceSet baseResourceSet, XtextResourceSet updatedResourceSet) {
        val baseFields = genUtils.getFieldsMap(baseResourceSet.resources)
        val updatedFieldIds = genUtils.getFieldsMap(updatedResourceSet.resources).keySet
        return baseFields.keySet
        .filter[id | !updatedFieldIds.contains(id)]
        .map[id | createError('''Message field «id» is deleted''', getCode)]
        .toList()
    }

}